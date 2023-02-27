using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Web.Api.DTOs.ResponseModels
{
    public class RoleResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
