using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Services.Role
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole<Guid>>> GetAll();
        Task<IdentityResult> Create(string roleName);
        Task<IdentityResult> Delete(string roleName);
        Task<IdentityResult> Update(Guid id, string updateRole);
    }
}