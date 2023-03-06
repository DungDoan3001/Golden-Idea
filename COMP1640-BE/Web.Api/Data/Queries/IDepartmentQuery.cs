using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface IDepartmentQuery
    {
        Task<List<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(Guid id);
    }
}