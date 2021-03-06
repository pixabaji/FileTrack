﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using FileTracking.ViewModels;

namespace FileTracking.Models
{
    public class DistrictToLocationValidation:ValidationAttribute
    {
        private ApplicationDbContext _context;

        public DistrictToLocationValidation()
        {
            _context = new ApplicationDbContext();
        }
        
        //our customer validation
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //return base.IsValid(value, validationContext);

            var file = (File)validationContext.ObjectInstance;
            if(file.DistrictsId == null && file.LocationId == null)
                return ValidationResult.Success;
            if(file.DistrictsId == null && file.LocationId != null)
                return new ValidationResult("A district must be provided before that matches the selected location.");
            if(file.DistrictsId != null && file.LocationId == null)
                return new ValidationResult("Ensure you provide a valid location associated with the selected district.");

            var fileLocation = _context.Locations.Single(l => l.LocationId == file.LocationId);
            bool error = false;
            if (file.LocationId != null && file.DistrictsId != 0)
            {
                if ( fileLocation.DistrictsId != file.DistrictsId)
                    error = true;
            }

            return (error == false) ? ValidationResult.Success : 
                new ValidationResult("Location does not match district. Please ensure your location's district match that from the drop down list above.");
        }
    }
}