using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Services.ResetPassword
{
    public interface IResetPasswordService
    {
        Task<Entities.ResetPassword> GetByIdAsync(Guid userId);
        Task<IEnumerable<Entities.ResetPassword>> FindByUserIdAsync(Guid userId);
        Task<Entities.ResetPassword> CreateAsync(Entities.ResetPassword resetPassword);
        Task<Entities.ResetPassword> UpdateAsync(Entities.ResetPassword resetPassword);
        Task<bool> DeleteAsync(Guid Id);
    }
}