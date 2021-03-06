﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileTracking.Models;
using FileTracking.ViewModels;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace FileTracking.Controllers
{
    public class FileVolumesController : Controller
    {
        private ApplicationDbContext _context;


        public FileVolumesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public void CreateNotificationForExConfirmation(Request req, string messageId)
        {
            var notif = new Notification()
            {
                RecipientUserId = req.UserId,
                MessageId = messageId,
                Read = false,
                //RequestId = req.Id,
                FileVolumeId = req.FileVolumesId,
                RecipientBranchId = req.RequesterBranchId,
                //SenderBranchId = req.RequesterBranchId,
                DateTriggered = DateTime.Now,
                SenderUserId = req.AcceptedById
            };

            _context.Notifications.Add(notif);
            _context.SaveChanges();
        }

        public void CreateNotification(Request req, string messageId)
        {
            var notif = new Notification()
            {
                RecipientUserId = req.UserId,
                MessageId = messageId,
                Read = false,
                //RequestId = req.Id,
                FileVolumeId = req.FileVolumesId,
                RecipientBranchId = req.RecipientBranchId,
                SenderBranchId = req.RequesterBranchId,
                DateTriggered = DateTime.Now,
                SenderUserId = req.AcceptedById
            };

            _context.Notifications.Add(notif);
            _context.SaveChanges();
        }
       
        public ActionResult VolHistory(int id, int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 10;//pagination set to 10
            var requestsInDb = _context.CompletedRequests.Include(r=>r.FileVolume).Include(r=>r.RequesterBranch)
                .Include(r=>r.RequesterUser).Include(r=>r.RegistryUserAccept).Include(r=>r.ReturnAcceptBy).Where(r => r.FileVolumeId == id).OrderByDescending(r=>r.RequestDate).ToList();

            PagedList<CompletedRequest> model = new PagedList<CompletedRequest>(requestsInDb, pageNum, pageSize);
            
            return PartialView("VolumeHistory", model);
        }

        // GET: FileVolumes for a specific file identified by the id parameter
        [Authorize(Roles = Role.RegularUser)]
        public ActionResult RequestFile(int id)
        {
            var thisUser = new AdUser(User.Identity.Name);
            var user = _context.AdUsers.Single(u => u.Username == thisUser.Username);

            if (user.IsDisabled)
            {
                return View("Locked");
            }

            var volFileId = _context.FileVolumes.Include(fv=>fv.States).
                Include(fv=>fv.Branches).Include(fv=>fv.CurrentLocation).Include(fv=>fv.AdUser).Where(fv => fv.FileId == id).ToList();

            var file = _context.Files.Include(f => f.FileVolumes).SingleOrDefault(f => f.Id == id );
            

            var viewModel = new VolumesViewModel()
            {
                File = file,
                FileVolumes = volFileId,
                AdUser = user
            };

            return View("FileVolume",viewModel);
        }

        [Authorize(Roles = Role.RegularUser)]
        public ActionResult UserVolumes()//this interface gets files currently check out to user
        {
            return View();
        }

        [Authorize(Roles = Role.RegularUser)]
        public ActionResult GetUserFileVolumes()
        {
            var currentUser = new AdUser(User.Identity.Name);
            var userInDb = _context.AdUsers.Single(u => u.Username == currentUser.Username);

            if (!userInDb.IsDisabled)
            {
                var request = _context.Requests.Include(r => r.FileVolumes).Include(r => r.RequesterBranch).Include(r => r.AcceptedBy).
                    Where(r => r.UserId == userInDb.Id).Where(r => r.IsRequestActive == true)
                    .Where(r => r.IsConfirmed == true).Where(r => r.ReturnStateId == 1).
                    Where(r => r.RequestTypeId == RequestType.InternalRequest  || r.RequestTypeId == RequestType.DirectTransfer).ToList();

                return Json(new { data = request }, JsonRequestBehavior.AllowGet);

            }

            return HttpNotFound("User Does not have permission to continue");
        }

        public JsonResult ReturnVolume(int id)
        {
            var request = _context.Requests.Single(r => r.Id == id);
 
            if (HasOtherTransfers(request.UserId, request.FileVolumesId))
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                request.ReturnStateId = 2;//2 means return stage is initiated, return sent
                _context.SaveChanges();

                CreateNotification(request, Message.Return);
                //since returning, state should transition to the transfer state. Just to give more detail. Not significant change
                ChangeStateToTransfer(request.FileVolumesId, request.UserId);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }          
        }

        //return file function after prompt was accepted
        public JsonResult VerifiedReturnOfVolume(int id)
        {
            var request = _context.Requests.Single(r => r.Id == id);

            request.ReturnStateId = 2;//2 means return is initiated, return sent

            _context.SaveChanges();

            CreateNotification(request, Message.Return);
            //since returning, state should transition to the transfer state. Just to give more detail. Not significant change
            ChangeStateToTransfer(request.FileVolumesId, request.UserId);

            //having a string value indicates we are asking for a list of objects to be returned from the dynamic type function
            //requests returned are those associated with our file
            var requestsToBeCanceled = HasOtherTransfers(request.UserId, request.FileVolumesId, "GetRequestObjects");
            CancelOtherTransferRequests(requestsToBeCanceled);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        //Checks if other transfers were made to the file we are returning.
        public dynamic HasOtherTransfers(int adUserId, int volumeId, string firstCheck = "")
        {
            var transferRequests = _context.Requests.Where(r => r.IsRequestActive == true).Where(r => r.FileVolumesId == volumeId)
                .Where(r => r.UserRequestedFromId == adUserId).Where(r => r.RequestStatusId == 1).ToList();

            if (firstCheck.IsNullOrWhiteSpace())
            {                
                if (transferRequests.Any())
                    return true;
                return false;
            }
            else
            {
                return transferRequests;
            }
        }

        public void CancelOtherTransferRequests(List<Request> requests)
        {
            var rejectedTransferOperations = new RejectedRequestController();
            foreach (var req in requests)
            {
                req.IsRequestActive = false;
                req.RequestStatusId = 3;//rejected state
                //ensure that on each request, a notification is dispatched to the respective user about file being rejected
                CreateNotificationForTransfers(req, Message.InReject);

                rejectedTransferOperations.SaveToRejectedRequestTable(req);
                _context.Requests.Remove(req);
                _context.SaveChanges();
            }
        }

        //creates a notification for transfer activities
        public void CreateNotificationForTransfers(Request req, string messageId)
        {
            var notif = new Notification()
            {
                RecipientUserId = req.UserId,
                MessageId = messageId,
                Read = false,
                FileVolumeId = req.FileVolumesId,
                RecipientBranchId = req.RecipientBranchId,
                SenderBranchId = req.RequesterBranchId,
                DateTriggered = DateTime.Now,
                SenderUserId = req.UserRequestedFromId
            };

            _context.Notifications.Add(notif);
            _context.SaveChanges();
        }
        
        //
        [Authorize(Roles = Role.Registry)]
        public ActionResult ReturnApproval()
        {
            return View("ReturnApproval");
        }

        [Authorize(Roles = Role.Registry)]
        public ActionResult GetReturnedRequests()
        {
            var username = new AdUser(User.Identity.Name);
            var adUser = _context.AdUsers.Single(a => a.Username == username.Username);
            var returnedReq = _context.Requests.Include(r => r.FileVolumes.Branches).
                Include(r => r.User.Branches).Where(r=>r.RecipientBranchId == adUser.BranchesId)
                .Where(r=>r.RequestTypeId == RequestType.InternalRequest || r.RequestTypeId == RequestType.DirectTransfer)
                .Where(r=>r.IsRequestActive == true).Where(r=>r.ReturnStateId == 2).ToList();


            return Json(new { data = returnedReq }, JsonRequestBehavior.AllowGet);
        }

        //handles returns for both external and internal transfer 
        public void AcceptReturn(int id)
        {
            var user = new AdUser(User.Identity.Name);//we get the current registry user and initialize its username
            var userInSessionInDb = _context.AdUsers.Single(u => u.Username == user.Username);

            var req = _context.Requests.Single(r => r.Id == id);
            
            req.ReturnedDate = DateTime.Now;
            req.ReturnAcceptById = userInSessionInDb.Id;
            req.ReturnStateId = 3;
            req.IsRequestActive = false;
            
            //if a binder exists then we activate the associated record 
            if (req.RequestBinder != 0)
            {
                //if it has external binder we switch the req type to external   
                _context.SaveChanges();
                InitiateExternalReturn(req.RequestBinder, req.FileVolumesId);//we set the external (based on the bind value) request to active
                UpdateVolumeForExternalTransfer(req.FileVolumesId);
            }
            else
            {
                _context.SaveChanges();
                ChangeStateToStored(req.FileVolumesId);               
            }
            //this request is finished no matter if associated with EXREQ, so we move to completed table
            var completedRequestOperation = new CompletedRequestController();
            completedRequestOperation.SaveToCompletedRequestTable(req);

            //we want to remove the record from Requests as we kinda duplicated the fields and stored in CompletedRequest
            _context.Requests.Remove(req);
            _context.SaveChanges();
        }

        //accepting external return
        [Authorize(Roles = Role.Registry)]
        public void AcceptExternalReturn(int id)
        {
           var user = new AdUser(User.Identity.Name);//we get the current registry user and initialize its username
           var userInSessionInDb = _context.AdUsers.Single(u => u.Username == user.Username);
            var req = _context.Requests.Single(r => r.Id == id);

            req.ReturnedDate = DateTime.Now;
            req.ReturnAcceptById = userInSessionInDb.Id;
            req.ReturnStateId = 3;
            req.IsRequestActive = false;

            _context.SaveChanges();

            ChangeStateToStored(req.FileVolumesId);
            CreateNotificationForExConfirmation(req,Message.ExReturnApproval);

            var completedRequestOperation = new CompletedRequestController();
            completedRequestOperation.SaveToCompletedRequestTable(req);

            _context.Requests.Remove(req);
            _context.SaveChanges();
        }

        public void ChangeStateToStored(int volId)
        {
            //stored state => 1
            var vol = _context.FileVolumes.Single(v => v.Id == volId);

            vol.StatesId = 1;
            vol.AdUserId = null;

            if(vol.CurrentLocationId != vol.BranchesId)//we both locations are different, it means it was an external request and we should reset since the return process is complete
                vol.CurrentLocationId = vol.BranchesId;
            _context.SaveChanges();
        }

        public void ChangeStateToTransfer(int volId, int requesterId)
        {
            //transfer = 4
            var vol = _context.FileVolumes.Single(v => v.Id == volId);

            vol.StatesId = 4;
            vol.AdUserId = requesterId;

            /*if (vol.CurrentLocation != vol.BranchesId)//we both locations are different, it means it was an external request and we should reset since the return process is complete
                vol.CurrentLocation = vol.BranchesId;*/
            _context.SaveChanges();
        }

        public void UpdateVolumeForExternalTransfer(int volId)
        {
            var vol = _context.FileVolumes.Single(v => v.Id == volId);

            vol.StatesId = 4;
            vol.AdUserId = null;

            _context.SaveChanges();
        }

        //after the bounded internal request is returned to local registry, we must initiate registry to registry request to return the file to its initial location
        public void InitiateExternalReturn(int binderId, int volumeId)
        {
            var extReturnReq = _context.Requests.Single(r=>r.RequestBinder == binderId && r.RequestTypeId == RequestType.ExternalRequest &&
                               r.RequestStatusId == 2 && r.FileVolumesId == volumeId && r.IsConfirmed == true);
            //if(extReturnReq == null)
                
            extReturnReq.IsRequestActive = true;
            _context.SaveChanges();
            return;
        }

        //load the return to branch view, registry users will be able to see external files currently at their disposal to further send a request back to the external registry
        [Authorize(Roles = Role.Registry)] 
        public ActionResult ReturnToBranch()
        {
            return View();
        }

        //executs query and returns external files list in order for the local registry to send external return
        public ActionResult GetReturnToBranchFiles()
        {
            var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            var request = _context.Requests.Include(r => r.FileVolumes).Include(r => r.User.Branches).Include(r=>r.RecipientBranch)
                .Where(r => r.RequesterBranchId == user.BranchesId).Where(r => r.RequestTypeId == RequestType.ExternalRequest).
                Where(r => r.IsConfirmed == true).Where(r=>r.ReturnStateId == 1).Where(r=>r.IsRequestActive == true).ToList();

            return Json(new { data = request }, JsonRequestBehavior.AllowGet);
        }

        public void ReturnToBranchAction(int id)
        {
            var currUser = new AdUser(User.Identity.Name);
            var userInSessionInDb = _context.AdUsers.Single(u => u.Username == currUser.Username);
            //return sent => 2
            var extReq = _context.Requests.Single(r => r.Id == id);
            extReq.ReturnStateId = 2;
            extReq.ReturnedDate = DateTime.Now;
            extReq.ReturnAcceptById = userInSessionInDb.Id;

            _context.SaveChanges();
            CreateNotification(extReq, Message.ExReturn);
        }

        [Authorize(Roles = Role.Registry)]
        public ActionResult ApproveExternalReturns()
        {
            return View();
        }

        public ActionResult GetExternalApproveReturns()
        {
            //after internal returns, local registry will need to return that file to its original branch
            //this query will return all those returns to its original branch to await confirmation
            var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            var request = _context.Requests.Include(r => r.FileVolumes).Include(r => r.User.Branches).Include(r=>r.RecipientBranch).
                Where(r => r.RecipientBranchId == user.BranchesId).Where(r => r.RequestTypeId == RequestType.ExternalRequest).
                Where(r => r.IsConfirmed == true).Where(r => r.ReturnStateId == 2).Where(r => r.IsRequestActive == true).ToList();

            return Json(new { data = request }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = Role.Registry)]
        public ActionResult UpdateVolumeDescription(int id)
        {
            var volumeInDb = _context.FileVolumes.Single(fv => fv.Id == id);

            return PartialView(volumeInDb);
        }

        [HttpPost]
        public ActionResult SaveVolumeDescription(FileVolumes fileVolumes)
        {
            if (!ModelState.IsValid)
            {
                //redirect to form page if validations fails with the same file vol objects redirected
                var volumeInDb = _context.FileVolumes.Single(fv => fv.Id == fileVolumes.Id);
                return PartialView("UpdateVolumeDescription", volumeInDb);
            }

            //otherwise, create a new volume record with respect to its file parent
            if (fileVolumes.Id != 0)
            {
                var volumeInDb = _context.FileVolumes.Single(v=>v.Id == fileVolumes.Id);

                //we only want to update comments (description) field
                volumeInDb.Comment = fileVolumes.Comment;

                _context.SaveChanges();

            }

            return RedirectToRoute(new
            {
                controller = "Files",
                action = "FileVolumes",
                id = fileVolumes.FileId
            });

        }

        //---------------------------Reclaim function -------------------------------------
        [Authorize(Roles = Role.Registry)]
        [Route("FileVolumes/ReclaimFileVolume/{volId}")]
        public JsonResult ReclaimFileVolume(int volId)
        {
            var userInAd = new AdUser(User.Identity.Name);
            var userInDb = _context.AdUsers.Single(u => u.Username == userInAd.Username);

            var volInDb = _context.FileVolumes.Single(v => v.Id == volId);

            //validate
            if(userInDb.BranchesId != volInDb.BranchesId)
                return Json(new { success = false, message = "Your branch does not correspond with the volume's branch and thus cannot continue" }, JsonRequestBehavior.AllowGet);
            if (volInDb.StatesId == 5 && volInDb.AdUserId != null) //5 => checked out
            {
                

                var reqInDb = _context.Requests.Single(r =>
                    r.FileVolumesId == volInDb.Id && r.UserId == volInDb.AdUserId && r.RequestStatusId == 2);
                //update the info to suite a check in
                reqInDb.ReturnedDate = DateTime.Now;
                reqInDb.ReturnAcceptById = userInDb.Id;
                reqInDb.ReturnStateId = 3;
                reqInDb.IsRequestActive = false;
                _context.SaveChanges();

                //end save mechanism

                ChangeStateToStored(reqInDb.FileVolumesId);
                //maybe a notification message should be here
                var completedRequestOperation = new CompletedRequestController();
                completedRequestOperation.SaveToCompletedRequestTable(reqInDb);

                _context.Requests.Remove(reqInDb);
                _context.SaveChanges();

                return Json(new { success = true, message = "Successfully reclaimed file!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "File volume is not in a valid state for this action to be processed." }, JsonRequestBehavior.AllowGet);
        }
    }
}