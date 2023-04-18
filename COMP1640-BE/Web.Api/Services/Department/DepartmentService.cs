using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Data.Queries;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;

namespace Web.Api.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Department> _departmentRepo;
        private readonly IDepartmentQuery _departmentQuery;

        public DepartmentService(IUnitOfWork unitOfWork, IDepartmentQuery departmentQuery)
        {
            _unitOfWork = unitOfWork;
            _departmentRepo = unitOfWork.GetBaseRepo<Department>();
            _departmentQuery = departmentQuery;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                return await _departmentQuery.GetAllAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> GetByIdAsync(Guid departmentId)
        {
            try
            {
                return await _departmentQuery.GetByIdAsync(departmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> CreateAsync(Department department)
        {
            try
            {
                Department createdDepartment = _departmentRepo.Add(department);
                await _unitOfWork.CompleteAsync();
                return createdDepartment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            try
            {
                Department updatedDepartment = _departmentRepo.Update(department);
                await _unitOfWork.CompleteAsync();
                return updatedDepartment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid departmentId)
        {
            try
            {
                bool isDelete = _departmentRepo.Delete(departmentId);
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

        public async Task<IEnumerable<Department>> GetByNameAsync(string name)
        {
            IEnumerable<Department> departments = await _departmentRepo.Find(x => x.Name == name);
            return departments;
        }
    }
}
