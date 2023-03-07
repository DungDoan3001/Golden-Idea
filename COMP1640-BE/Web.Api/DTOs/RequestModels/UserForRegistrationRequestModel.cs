using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Web.Api.Extensions;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForRegistrationRequestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [MaxFileSize(10 * 1024 * 1024)]
        public IFormFile Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public Guid DepartmentId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
