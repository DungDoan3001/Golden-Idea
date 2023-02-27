using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForRegistrationRequestModel
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public string Role { get; set; }
    }
}
