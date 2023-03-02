using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.User
{
    public interface IUserService
    {
        Task<List<Entities.User>> GetAll();
        Task<List<Entities.User>> GetAllAdmin();
        Task<List<Entities.User>> GetAllStaffQA();
        Task<Entities.User> GetById(Guid id);
        Task<Entities.User> UpdateAsync(Guid id, UserRequestModel user);
        Task<IdentityResult> Delete(Guid id);
    }
}
