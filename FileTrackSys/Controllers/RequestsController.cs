﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FileTracking.Models;
using FileTracking.ViewModels;
using Microsoft.AspNet.Identity;

namespace FileTracking.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext _context;

        public RequestsController()
        {
            _context = new ApplicationDbContext();
       
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public void CreateNotification(Request req, string messageId)
        {
            var notif = new Notification()
            {
                RecipientUserId = req.UserId,
                MessageId = messageId,
                Read = false,
                RequestId = req.Id,
                DateTriggered = DateTime.Now,
                SenderUser = req.AcceptedBy
            };

            _context.Notifications.Add(notif);
            _context.SaveChanges();
        }

        public ActionResult VolumeStateNotValid()
        {
            return View();
        }

        //will check if user's branch matches the files current location
        public bool CheckBranchValidity(AdUser u, FileVolumes v)
        {
            if (u.BranchesId == v.CurrentLocation)
                return true;
            return false;
        }

        //this is invoked whenever a user makes a request from the 'FileVolume' page where the parameters with the exact volume is provided.
        [Route("Requests/Index/{volId}")]
        public ActionResult Index(int volId)
        {
            //pulls records associated with a request such as volume and user
            var userObj = new AdUser(User.Identity.Name);          

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            if (user.IsDisabled == false)
            {
                var volume = _context.FileVolumes.Single(v => v.Id == volId);

                //check if this volume number has not already been requested by this user
                if (volume.StatesId == 1)
                {
                    if (HasBeenRequested(volume, user))
                        return View("AlreadyRequested");
                }

                if (CheckBranchValidity(user, volume))
                {
                    if (PopulateRequest(volume, user))
                    {
                        UpdateVolumeState(volume);
                        return View();
                    }
                    else
                        return Content("Saving to request database failed");

                }
                else
                {
                    if (PopulateExternalRequests(volume, user))
                        return View("ExternalRequestMade");
                    else
                    {
                        return Content(
                            "Sorry it appears something occured with the methods of inserting into your database. Check your query or logic");
                    }
                }
            }

            return View("Locked");
        }

        //checks that a volume is not requested more than once by the same user
        public bool HasBeenRequested(FileVolumes v, AdUser u)
        {
          //First we query based in current user, then we get the volume id, and finally if the request is active.
            var userReq = _context.Requests.Where(r => r.UserId == u.Id).Where(r => r.FileVolumesId == v.Id)
                .Where(r=>r.IsRequestActive == true).ToList();

            if (userReq.Any())
                return true;
            return false;
        }
        //populates request table with data
        public bool PopulateRequest(FileVolumes v, AdUser u)
        {
            var requestRecord = new Request()
            {
                //FileId = f.Id,
                UserId = u.Id,
                FileVolumesId = v.Id,
                RequesteeBranch = v.CurrentLocation,//the requesteeBranch is the brnacch where the request will be sent to, based in the volume's current location
                BranchesId = u.BranchesId,//we get the branch based on the signed on user, since user must match volume branch atm
                RequestStatusId = 1, //1 signifies pending
                ReturnStateId = 1,//1 signifies idle state, meaning the return process is not in order
                IsRequestActive = true,//meaning this request has been initiated, switched to false when file is returned and back to stored state
                RequestDate = DateTime.Now,//assigns immediate date as the request was made in this moment of time
                RequestTypeId = RequestType.InternalRequest
            };
            _context.Requests.Add(requestRecord);
            
            if (_context.SaveChanges() > 0)
                return true;
            
            return false;
        }

        public bool PopulateExternalRequests(FileVolumes v, AdUser u)
        {
            //line below holds a value we will use to bind both requests and then increment thereafter assignment.
            var bindVal = _context.ExternalRequestsBinder.Single(b => b.Id == 1);

            var externalRequestRec = new Request()
            {
                UserId = u.Id,//0 doesn't work so we try must use another determining value
                FileVolumesId = v.Id,
                RequesteeBranch = v.CurrentLocation,//In this case requestee branch is assigned to the external location that corresponds to the vol loc
                BranchesId = u.BranchesId,//requesting user's location which will ofc differ from requestee branch
                RequestStatusId = 1, //1 signifies pending
                ReturnStateId = 1,//1 signifies idle state, meaning the return process is not in order
                IsRequestActive = true,//meaning this request has been initiated, switched to false when file is returned and back to stored state
                RequestDate = DateTime.Now,
                RequestBinder = bindVal.CurrentNumberBinder,//This identifier will bindd these two newly created requests
                RequestTypeId = RequestType.ExternalRequest
            };          
             
            //for the above 2 requests we must have a binding value, a unique identifier that only those 2 will have to know to move on to the next request after the first is executed            
             _context.Requests.Add(externalRequestRec);
             
             bindVal.CurrentNumberBinder++;//we increment the value in order to generate a unique value on another instance
             if (_context.SaveChanges() > 0)//confirms changes
                 return true;
             return false;
        }

        //updates the state of the volume being requested
        public void UpdateVolumeState(FileVolumes v)
        {
            if (v.StatesId != 1) //1 meaning state is at stored
            {
                v.StatesId = 1; //state should now be changed to 2 (requested state) since file request is made
                _context.SaveChanges();
            }
            //if it is not in STORED state, it should already be in request state as other user may have already requested thus changing 
            //the state then. In that case we should simply do nothing
        }

        // Direct to pendingFiles page (RegistryOnly)
        [Authorize(Roles = Role.Registry)]
        public ActionResult PendingFiles()
        {           
            var currentUser = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == currentUser.Username);

            if (user.IsDisabled)
                return View("Locked");

            return View();
        }

        //sends all request records with a Request status of pending to be approved by registry
        [Authorize(Roles = Role.Registry)]
        public ActionResult GetPendingFiles()
        {
            //for the most part, requesteeBranchId will always be the currentLocation Branch
            //we must ensure to take into account branches. registry is only to see request from user made within their respective branch
            var userObj = new AdUser(User.Identity.Name);
           
            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            if (user.IsDisabled)
                return View("Locked");
            
            //PENDING = 1 (RequestStatusID)
            //where a User RequesteeFrom field is NULL, Registry users in general are represented
            var pendingRequests = _context.Requests.Include(r => r.FileVolumes).
                Include(r => r.User.Branches).Where(r=>r.RequesteeBranch == user.BranchesId).Where(r => r.RequestStatusId == 1).
                Where(r=>r.IsRequestActive == true).Where(r=>r.RequestTypeId == RequestType.InternalRequest)
                .Where(r => r.UserRequestedFromId == null).ToList();

            return Json(new { data = pendingRequests }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = Role.Registry)]
        public ActionResult ExternalBranchRequest()
        {
            return View();
        }

        //Gets files that are awaiting external approval
        [Authorize(Roles = Role.Registry)]
        public ActionResult GetExternalBranchPendingFiles()
        {
            var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);
      
            byte Pending = 1;
            var pendingRequests = _context.Requests.Include(r => r.FileVolumes).
                Include(r => r.User.Branches).Where(r=>r.RequesteeBranch == user.BranchesId).Where(r => r.IsRequestActive == true).
                Where(r => r.RequestStatusId == Pending || r.RequestStatusId == 4)
                .Where(r => r.RequestTypeId == RequestType.ExternalRequest).ToList();

            
            return Json(new { data = pendingRequests }, JsonRequestBehavior.AllowGet);
        }

        //check if a requested state has not been already checked out or in the transfer state
        public bool IsVolumeStateValid(int volId)
        {
            /*var req = _context.Requests.Where(r => r.RequestTypeId == RequestType.ExternalRequest).
                Where(r => r.IsRequestActive == true).Where(r => r.FileVolumesId == volId).Where(r => r.RequestStatusId == 2).ToList();*/
            var volumeInDb = _context.FileVolumes.Single(v=>v.Id == volId);
            if (volumeInDb.StatesId != 1) {
                return false;
            }
            return true;
        }

        //if in fact other multiple requests were made for the same volume and one gets accepted we cancel the others
        public void CancelOtherRequestsForVolume(int volId, int excludeReq)
        {
            var cancelReq = _context.Requests.Where(r => r.FileVolumesId == volId).Where(r => r.IsRequestActive == true)
                .Where(r=>r.RequestStatusId == 1).ToList();

            foreach (var req in cancelReq)
            {
                req.IsRequestActive = false;//here we also need to update the notification system when its implemented (for a later time)
                req.RequestStatusId = 3; //we set the request status to rejected
            }

            _context.SaveChanges();
        }

        //function is invoked when local registry user accepts a file, changing it to check out
        public JsonResult AcceptRequest(int id)
        {            
            var userObj = new AdUser(User.Identity.Name);
            //recall that a person from registry accepts this request so get person name
            const byte acceptedState = 2;
            var request = _context.Requests.Single(r => r.Id == id);
            if (!IsVolumeStateValid(request.FileVolumesId))
            {
                CancelOtherRequestsForVolume(request.FileVolumesId, id);
                return this.Json(new {success = false }, JsonRequestBehavior.AllowGet);//we return a false ajax request
            }
            
                request.RequestStatusId = acceptedState;
                request.AcceptedBy = userObj.Username;
                request.AcceptedDate = DateTime.Now;
                //request.UserId =

                _context.SaveChanges();
                UpdateVolumeState(request.FileVolumesId);

                CreateNotification(request, Message.InAccept);
                return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //for external requests, as opposed to the above's local request
        public ActionResult AcceptExternalUserRequest(int id)
        {
            var userObj = new AdUser(User.Identity.Name);
            //recall that a person from registry accepts this request so get person name
            const byte acceptedState = 2;
            var request = _context.Requests.Single(r => r.Id == id);
            if (request.RequestStatusId != 4)
            {
                if (!IsVolumeStateValid(request.FileVolumesId))
                {
                    CancelOtherRequestsForVolume(request.FileVolumesId, id);
                    return this.Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }

           
                request.RequestStatusId = acceptedState;
                request.AcceptedBy = userObj.Username;
                request.AcceptedDate = DateTime.Now;
                //request.UserId = 
                int volId = request.FileVolumesId;

                _context.SaveChanges();
                UpdateVolumeState(volId);
            //Since the 
            CreateNotification(request, Message.ExAccept);
            return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //note tha once a file vol is accepted by reg that vol should no longer be in a stored or requested state but
        //rather a transferred state
        public void UpdateVolumeState(int volumeId)
        {
            //Request State 4 => transfer state
            var volume = _context.FileVolumes.Single(v => v.Id == volumeId);
            if (volume.StatesId == 4)
            {
                return;
            }
            volume.StatesId = 4;

            _context.SaveChanges();
        }

        //here we accept the request id and change its status to Deny/reject
        public void DeclineRequest(int id)
        {
            const byte rejectedState = 3;
            var request = _context.Requests.Single(r => r.Id == id);

            request.RequestStatusId = rejectedState;
            request.IsRequestActive = false;

            _context.SaveChanges();
            if (request.RequestBinder == 0)
            {
                CreateNotification(request, Message.InReject);
            }
            else if(request.RequestTypeId == RequestType.ExternalRequest)
            {
                CreateNotification(request, Message.ExReject);
            }      
            //say for instance registry rejects a file, recall the request nonetheless changed the volume state to 
            //requested, when we deny a request we do not perform any changing of state so what operation resolves the issue.
            //after a volume's been rejected, better yet, let all request for that specific volume be denied.
            //What do we do then? since the state will remain at requested and never changed due to it never being accepted.
            //does this affect the flow of things
        }

        [Authorize(Roles = Role.Registry)]
        public ActionResult ExternalTransferApproval()
        {
            return View();
        }

        //After getting external approval a file must then make its way back to internal registry to gain further confirmation
        [Authorize(Roles = Role.Registry)]
        public ActionResult GetExternalTransferRecords()
        {

            var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            var request = _context.Requests.Include(r => r.FileVolumes).Include(r => r.User.Branches).Include(r=>r.User).
                Where(r=>r.BranchesId == user.BranchesId).Where(r=>r.RequestTypeId == RequestType.ExternalRequest).
                Where(r => r.RequestStatusId == 2).Where(r => r.IsConfirmed == false).ToList();

            return Json(new { data = request }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = Role.RegularUser)]
        public ActionResult ConfirmCheckout()
        {
            var currentUser = new AdUser(User.Identity.Name);
            try
            {
                var userInDb = _context.AdUsers.Single(u => u.Username == currentUser.Username);

                if (userInDb.IsDisabled == false)
                {
                    return View();
                }

                return View("Locked");
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }

        }

        //where REGULAR USERS will be able to see those requests accepted by registry
        [Authorize(Roles = Role.RegularUser)]
        public ActionResult GetConfirmCheckout()
        {
            byte reqStatus = 2;

           var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            var request = _context.Requests.Include(r=>r.FileVolumes).Include(r=>r.Branches).Where(r => r.UserId == user.Id).Where(r=>r.IsRequestActive == true)
                .Where(r=>r.RequestTypeId == RequestType.InternalRequest).Where(r=>r.RequestStatusId == reqStatus).Where(r=>r.IsConfirmed == false).
                Where(r=>r.UserRequestedFromId == null).ToList();
            //UserRequestedFromId = = NULL signifies we only get accepted files from REGISTRY.
            return Json(new { data = request }, JsonRequestBehavior.AllowGet);
            
        }

        public void AcceptCheckout(int id)
        {
            //search in request table based on the id parameter
            //change IsConfirmed field to true, meaning file is now checked out by user
            //call and implement a function that updates the file volumes table, specifically the state to the volume
            //from TRANSFERRED --> CHECKED OUT

            //END INTERFACE
            var requestRecord = _context.Requests.Single(r => r.Id == id);

            requestRecord.IsConfirmed = true;
            _context.SaveChanges();

            CheckoutVolume(requestRecord.FileVolumesId, requestRecord.UserId);
            //perhaps here we create a function that performs notifications

        }

        //the external registry interaction confirmation for file transfer
        public void AcceptForeignRegistryTransfer(int id)
        {
            //request binded records that sees to registry to registry operations before initiating the local request it is bound by
            var requestRecord = _context.Requests.Single(r => r.Id == id);
          
            requestRecord.IsConfirmed = true;
            requestRecord.ReturnStateId = 1;
            requestRecord.IsRequestActive = false;
            _context.SaveChanges();

            //after the above is performed we let its binded request become active in order for the initiating user to go through the local request process
            //also be reminded to change to volume's current location, as being accepted now signifies the file is at a foreign branch       

           var volume = _context.FileVolumes.Single(v => v.Id == requestRecord.FileVolumesId);

           PopulateNewInternalRequest(requestRecord.UserId, requestRecord.BranchesId, requestRecord.RequestDate,volume, requestRecord.RequestBinder);//both registry passed, now we create an internal request

           volume.CurrentLocation = requestRecord.BranchesId; //we must change the volume's current location to the current user's branch
           _context.SaveChanges();
           CreateNotification(requestRecord, Message.InAccept);//revise
        }

        //now that user's local branch has accepted 
        public void PopulateNewInternalRequest(int userId, byte userBranchId, DateTime reqDate,FileVolumes v, int binder)
        {
            var user = new AdUser(User.Identity.Name);
            var internalRequest = new Request()
            {
                UserId = userId,//the user's, making the request, id
                FileVolumesId = v.Id,
                RequesteeBranch = userBranchId,//In this case requestee branch is assigned to user making the request's branch, as he must request from his respective branch eventually
                BranchesId = userBranchId,//requesting user's location which will ofc differ from requestee branch
                RequestStatusId = 2, //2 signifies accepted
                ReturnStateId = 1,//1 signifies idle state, meaning the return process is not in order
                IsRequestActive = true,//Since this vol is depending on the above request to be confrimed, it's default val is false, until the above get confirmed
                RequestDate = reqDate,
                RequestBinder = binder,
                RequestTypeId = RequestType.InternalRequest,
                AcceptedBy = user.Username,
                AcceptedDate = DateTime.Now
            };
            _context.Requests.Add(internalRequest);
            _context.SaveChanges();

        }

        public void NeverReceived(int id)
        {
            var requestRecord = _context.Requests.Single(r => r.Id == id);
            requestRecord.RequestStatusId = 4;//we will change to never received change, furthermore we create a duplicate record of this one
            //with exception of the request status being changed to pending so another request be sent to external registry once more
            requestRecord.IsRequestActive = false;
            _context.SaveChanges();

            var newRequestRecord = requestRecord;//creating a new request so the prior registry may receive the request again. Duplicate
        
            newRequestRecord.RequestStatusId = 4;
            newRequestRecord.AcceptedBy = null;
            newRequestRecord.AcceptedDate = null;
            newRequestRecord.IsRequestActive = true;
            _context.Requests.Add(newRequestRecord);
            _context.SaveChanges();

            //UpdateVolumeStateStored(requestRecord.FileVolumesId);
        }

        public void CheckoutVolume(int volId, int requester)
        {          
            const byte checkoutState = 5;
            var volume = _context.FileVolumes.Single(v => v.Id == volId);
            volume.StatesId = checkoutState;
            volume.AdUserId = requester; 
            _context.SaveChanges();
        }

        public void UpdateVolumeStateStored(int volumeId)
        {
            const byte stored = 1;
            var volume = _context.FileVolumes.Single(v => v.Id == volumeId);
            volume.StatesId = stored;

            _context.SaveChanges();
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------Initiating User Transfer functionality ----------------------------------------------------------- 
        //----------------------------------------------------------------------------------------------------------------------------------
        
        //When a user makes attempts to have a file transferred to them this function is invoked to create a new record
        [Authorize(Roles = Role.RegularUser)]
        [Route("Requests/OnUserTransferAccept/{volId}/{userId}/{currentLocation}")]
        public ActionResult OnUserTransferAccept(int volId, int userId, byte currentLocation)
        {
            //user in session
            var userObj = new AdUser(User.Identity.Name);

            var thisUser = _context.AdUsers.Single(u => u.Username == userObj.Username);

            //Checks if user account in session is not disabled
            if (thisUser.IsDisabled)
                return View("Locked");

            //checks that user in session is not requesting from his/her own self
            if (thisUser.Id == userId)
                return HttpNotFound("It appears your request of transfer is being processed to yourself. Cannot proceed.");

            var volInDb = _context.FileVolumes.Single(v => v.Id == volId);

            //checks is file volumes is eligible for a transfer
            if (volInDb.StatesId != 5 || volInDb.AdUserId == null)
                return HttpNotFound("Cannot make this requests as the file as the file is not at the hands of the requestee. ");

            //creating that new request record
            var newRequest = new Request()
            {
                UserId = thisUser.Id,//this is the new user making the request
                FileVolumesId = volId,
                RequesteeBranch = currentLocation,//the requesteeBranch is the brnacch where the request will be sent to, based in the volume's current location
                BranchesId = thisUser.BranchesId,//we get the branch based on the signed on user, since user must match volume branch atm
                RequestStatusId = 1, //1 signifies pending
                ReturnStateId = 1,//1 signifies idle state, meaning the return process is not in order
                IsRequestActive = true,//meaning this request has been initiated, switched to false when file is returned and back to stored state
                RequestDate = DateTime.Now,//assigns immediate date as the request was made in this moment of time
                RequestTypeId = RequestType.InternalRequest,
                UserRequestedFromId = userId //THIS is the user from who the transfer is being requested
            };

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            return View("RequestSuccessful"); //user file transfer button functionality      
        }

        //the view page function
        [Authorize(Roles = Role.RegularUser)]
        public ActionResult UserPendingTransfer()
        {
            return View();//Navigational link to page
        }

        //GET: a user's transfers that are pending so the user currently with the file may choose to accept or deny
        [Authorize(Roles = Role.RegularUser)]
        public ActionResult GetUserPendingTransfers()
        {
            var userObj = new AdUser(User.Identity.Name);

            var user = _context.AdUsers.Single(u => u.Username == userObj.Username);

            if (user.IsDisabled)
                return View("Locked");
            
            var request = _context.Requests.Include(r => r.FileVolumes).Include(r=>r.Branches).Include(r => r.UserRequestedFrom).Include(r=>r.User)
                .Where(r=>r.UserRequestedFromId == user.Id).Where(r=>r.IsRequestActive == true).Where(r=>r.RequestStatusId == 1).ToList();

            return Json(new { data = request }, JsonRequestBehavior.AllowGet);
        }

        //user accepts the pending transfer
        [Authorize(Roles = Role.RegularUser)]
        public JsonResult AcceptTransfer(int id)
        {
            var userInSession = new AdUser(User.Identity.Name);

            //the main request to be worked with
            var requestInDb = _context.Requests.Single(r => r.Id == id);

            var volumeInDb = _context.FileVolumes.Single(v => v.Id == requestInDb.FileVolumesId);

            //CHECKED OUT state = 5.
            //Recall a file must be checked out to a regular user (Will specify Id, so not NULL is the criteria) in order for transfer to be possible
            if (volumeInDb.AdUserId != null && volumeInDb.StatesId == 5)
            {
                //we are getting the request for the holding user to transfer some content 
                var holdingUserReq = _context.Requests.Single(r => r.UserId == requestInDb.UserRequestedFromId 
                                     && r.IsRequestActive == true &&  r.FileVolumesId == requestInDb.FileVolumesId);

                holdingUserReq.IsRequestActive = false;

                //Set new record Accept state criteria for the user the file is to be transferred to. 
                requestInDb.RequestStatusId = 2;
                requestInDb.AcceptedDate = DateTime.Now;
                requestInDb.AcceptedBy = userInSession.Username;

                //create ACCEPT NOTIFICATION
                var acceptNotif = new Notification()
                {
                    RecipientUserId = requestInDb.UserId,
                    MessageId = Message.InAccept,
                    Read = false,
                    RequestId = requestInDb.Id,
                    DateTriggered = DateTime.Now,
                    SenderUser = requestInDb.AcceptedBy
                };

                _context.Notifications.Add(acceptNotif);

                //canceling other request to the same file. Except the req id just granted
                CancelOtherTransferRequests(volumeInDb, requestInDb.Id);

                //now we change the file volume to TRANSFER state (4) since transfer was granted
                volumeInDb.StatesId = 4;
                _context.SaveChanges();

                return this.Json(new { success = true, message = "Transfer successfully accepted" }, JsonRequestBehavior.AllowGet);                      
            }
            return this.Json(new { success = false, message = "It appears the file has been sent back to registry and cannot commit to transfer." }, JsonRequestBehavior.AllowGet);
        }

        //will cancel all other requests to the same file once a request is accepted
        [Authorize(Roles = Role.RegularUser)]
        public void CancelOtherTransferRequests(FileVolumes fv, int reqId)
        {
            //cancel every other request record except for the provided in the parameter
            var reqsInDb = _context.Requests.Where(r => r.UserRequestedFromId == fv.AdUserId).Where(r => r.Id != reqId)
                .Where(r => r.FileVolumesId == fv.Id).ToList();

            foreach (var req in reqsInDb)
            {
                req.IsRequestActive = false;
                //request record will be set to rejected = 3
                req.RequestStatusId = 3;

                //we must also make a function that creates a new record in the notifications table, informing th user their file got canceled.
                NotifyUserOfTransferDenials(req,Message.InReject);
            }
           
        }

        //Notification of requests being discarded.
        public void NotifyUserOfTransferDenials(Request req, string messageId)
        {
            //REJ - that a file's been rejected
            var notif = new Notification()
            {
                RecipientUserId = req.UserId,
                MessageId = messageId,
                Read = false,
                RequestId = req.Id,
                DateTriggered = DateTime.Now,
                SenderUser = req.AcceptedBy
            };
           _context.Notifications.Add(notif);
        }

        //Creating view navigation Page ---------------------------------------------------------------------
        [Authorize(Roles = Role.RegularUser)]
        public ActionResult UserConfirmTransfer()
        {
            return View();
        }

        //Json request function that returns a serialized list of request objects
        [Authorize(Roles = Role.RegularUser)]
        public JsonResult GetConfirmationTransfers()
        {
            var userInSession = new AdUser(User.Identity.Name);

            var adUserInDb = _context.AdUsers.Single(u => u.Username == userInSession.Username);

            var requestsInDb = _context.Requests.Include(r=>r.User).Include(r=>r.FileVolumes).Include(r=>r.Branches).
                Where(r => r.UserId == adUserInDb.Id).Where(r => r.IsConfirmed == false)
                .Where(r => r.IsRequestActive == true).ToList();

            return Json(new { data = requestsInDb }, JsonRequestBehavior.AllowGet);
        }
    }

}