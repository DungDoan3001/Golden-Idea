using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Context;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public class CategoryQuery : BaseQuery<Category>, ICategoryQuery
    {
        public CategoryQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Category>> GetAllAsync()
        {
            return await dbSet
                .Include(x => x.Ideas)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await dbSet
                .Include(x => x.Ideas)
                .Where(x => x.Id == id)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
}
