using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Services.Category
{
    public interface ICategoryService
    {
        Task<Entities.Category> CreateAsync(Entities.Category category);
        Task<bool> DeleteAsync(Guid categoryId);
        Task<IEnumerable<Entities.Category>> GetAllAsync();
        Task<Entities.Category> GetByIdAsync(Guid categoryId);
        Task<IEnumerable<Entities.Category>> GetByNameAsync(string name);
        Task<Entities.Category> UpdateAsync(Entities.Category category);
    }
}