using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Api.Entities;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForRegistrationRequestModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> Department { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
