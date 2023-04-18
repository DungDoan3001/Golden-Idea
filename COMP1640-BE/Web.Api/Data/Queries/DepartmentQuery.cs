using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Api.Data.Context;
using Web.Api.Entities;
using System.Linq;

namespace Web.Api.Data.Queries
{
    public class DepartmentQuery : BaseQuery<Department>, IDepartmentQuery
    {
        public DepartmentQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Department>> GetAllAsync()
        {
            return await dbSet
                .Include(x => x.Users)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Department> GetByIdAsync(Guid id)
        {
            return await dbSet
                .Include(x => x.Users)
                .Where(x => x.Id == id)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
}
