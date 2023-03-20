using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Web.Api.Data.Context;
using Web.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Data.Queries
{
    public class ViewQuery : BaseQuery<View>, IViewQuery
    {
        public ViewQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<View>> GetAllByIdeaAsync(Guid ideaId)
        {
            return await dbSet
                .Where(x => x.IdeaId == ideaId)
                .ToListAsync();
        }
    }
}
