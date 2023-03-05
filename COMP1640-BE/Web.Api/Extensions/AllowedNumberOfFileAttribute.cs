using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web.Api.Extensions
{ 
    public class AllowedNumberOfFileAttribute : ValidationAttribute
    {
        private readonly int _numberOfFile;
        public AllowedNumberOfFileAttribute(int numberOfFile)
        {
            _numberOfFile = numberOfFile;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as List<IFormFile>;
            if (file != null)
            {
                if (file.Count() > _numberOfFile)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed number of file is {_numberOfFile}.";
        }

    }
}
