using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Web.Api.Entities;

namespace Web.Api.DTOs.ResponseModels
{
    public class UserForRegistrationResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public ICollection<string> Roles { get; set; } = new Collection<string>();
    }
}
