using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<Department> CreateAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<bool> DeleteAsync(Guid departmentId);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(Guid departmentId);
        Task<IEnumerable<Department>> GetByNameAsync(string name);
    }
}