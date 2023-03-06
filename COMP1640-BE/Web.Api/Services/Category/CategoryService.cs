using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;
using Web.Api.Data.Queries;

namespace Web.Api.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Entities.Category> _categoryRepo;
        private readonly ICategoryQuery _categoryQuery;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryQuery categoryQuery)
        {
            _unitOfWork = unitOfWork;
            _categoryRepo = unitOfWork.GetBaseRepo<Entities.Category>();
            _categoryQuery = categoryQuery
        }

        public async Task<IEnumerable<Entities.Category>> GetAllAsync()
        {
            try
            {
                return await _categoryQuery.GetAllAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Category> GetByIdAsync(Guid categoryId)
        {
            try
            {
                return await _categoryQuery.GetByIdAsync(categoryId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Category> CreateAsync(Entities.Category category)
        {
            try
            {
                Entities.Category createdCategory = _categoryRepo.Add(category);
                await _unitOfWork.CompleteAsync();
                return createdCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Category> UpdateAsync(Entities.Category category)
        {
            try
            {
                Entities.Category updatedCategory = _categoryRepo.Update(category);
                await _unitOfWork.CompleteAsync();
                return updatedCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            try
            {
                bool isDelete = _categoryRepo.Delete(categoryId);
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

        public async Task<IEnumerable<Entities.Category>> GetByNameAsync(string name)
        {
            IEnumerable<Entities.Category> categories = await _categoryRepo.Find(x => x.Name == name);
            return categories;
        }
    }
}
