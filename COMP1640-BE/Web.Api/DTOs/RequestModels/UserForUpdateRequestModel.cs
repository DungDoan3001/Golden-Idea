using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using Web.Api.Extensions;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForUpdateRequestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [MaxFileSize(10 * 1024 * 1024)]
        public IFormFile File { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public Guid DepartmentId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
