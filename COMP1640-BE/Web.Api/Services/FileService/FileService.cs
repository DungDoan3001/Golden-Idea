using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> CheckExistedFilePathContainDuplicateAsync(string filePath)
        {
            var files = await _fileRepo.Find(x => x.FilePath == filePath);
            if (files.Count() > 1)
                return true;
            return false;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<File> entities)
        {
            try
            {
                var result = _fileRepo.DeleteRange(entities);
                await _unitOfWork.CompleteAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var result = _fileRepo.Delete(id);
                await _unitOfWork.CompleteAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
