using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;
using System.Collections.Generic;

namespace Web.Api.Services.ResetPassword
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Entities.ResetPassword> _resetPasswordRepo;

        public ResetPasswordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _resetPasswordRepo = unitOfWork.GetBaseRepo<Entities.ResetPassword>();
        }
        public async Task<Entities.ResetPassword> GetByIdAsync(Guid resetPasswordId)
        {
            try
            {
                return await _resetPasswordRepo.GetById(resetPasswordId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Entities.ResetPassword>> FindByUserIdAsync(Guid userId)
        {
            IEnumerable<Entities.ResetPassword> resetPassword = await _resetPasswordRepo.Find(x => x.UserId == userId);
            return resetPassword;
        }
        public async Task<Entities.ResetPassword> CreateAsync(Entities.ResetPassword resetPassword)
        {
            try
            {
                Entities.ResetPassword createdResetPassword = _resetPasswordRepo.Add(resetPassword);
                await _unitOfWork.CompleteAsync();
                return createdResetPassword;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Entities.ResetPassword> UpdateAsync(Entities.ResetPassword resetPassword)
        {
            try
            {
                Entities.ResetPassword updatedResetPassword = _resetPasswordRepo.Update(resetPassword);
                await _unitOfWork.CompleteAsync();
                return updatedResetPassword;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            {
                bool isDelete = _resetPasswordRepo.Delete(Id);
                if (isDelete)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
