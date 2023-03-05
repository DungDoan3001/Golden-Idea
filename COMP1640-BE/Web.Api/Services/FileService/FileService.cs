using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;

namespace Web.Api.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<File> _fileRepo;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _fileRepo = unitOfWork.GetBaseRepo<File>();
        }

        public async Task<IEnumerable<File>> AddRangeAsync(IEnumerable<File> entities)
        {
            try
            {
                var addedFiles = _fileRepo.AddRange(entities);
                await _unitOfWork.CompleteAsync();
                return addedFiles;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
