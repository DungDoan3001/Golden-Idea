using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface ICategoryQuery
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);
    }
}