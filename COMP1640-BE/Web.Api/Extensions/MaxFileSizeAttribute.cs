using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web.Api.Extensions
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value != null)
            {
                Console.WriteLine(value.GetType());
                Console.WriteLine(typeof(FormFile));
                if (value.GetType() == typeof(FormFile))
                {
                    var file = value as IFormFile;
                    if (file != null)
                    {
                        if (file.Length > _maxFileSize)
                        {
                            return new ValidationResult(GetErrorMessage());
                        }
                    }
                }
                else if (value.GetType() == typeof(List<IFormFile>))
                {
                    var files = value as List<IFormFile>;
                    foreach (var file in files)
                    {
                        if (file.Length > _maxFileSize)
                        {
                            return new ValidationResult(GetErrorMessage());
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSize} bytes.";
        }
    }
}
