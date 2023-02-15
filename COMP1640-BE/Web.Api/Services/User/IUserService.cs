using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseModel>> GetAll();
        Task<Entities.User> UpdateAsync(Guid id, Entities.User user);
        Task<IdentityResult> Delete(Guid id);
    }
}
