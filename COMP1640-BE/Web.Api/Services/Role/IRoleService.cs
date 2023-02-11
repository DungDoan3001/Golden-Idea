using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.Role
{
    public interface IRoleService
    {
        Task<List<RoleResponseModel>> GetAll();
        Task<RoleResponseModel> Create(string roleName);
        Task<IdentityResult> Delete(string roleName);
        Task<RoleResponseModel> Update(Guid id, string updateRole);
    }
}