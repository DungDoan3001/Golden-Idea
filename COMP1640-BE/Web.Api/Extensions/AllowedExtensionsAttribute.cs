using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Web.Api.Extensions
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                if(value.GetType() == typeof(FormFile))
                {
                    var file = value as IFormFile;
                    if (file != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!_extensions.Contains(extension.ToLower()))
                        {
                            return new ValidationResult(GetErrorMessage());
                        }
                    }
                } else if (value.GetType() == typeof(List<IFormFile>))
                {
                    var files = value as List<IFormFile>;
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!_extensions.Contains(extension.ToLower()))
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
            var stringBuilder = new StringBuilder();

            foreach (var item in _extensions)
            {
                stringBuilder.Append(item).Append(" ");
            }
            return $"The file extension is not allowed! Please try again with: " + stringBuilder.ToString();
        }
    }
}
