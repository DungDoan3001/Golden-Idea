using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public class IdeaQuery : BaseQuery<Idea>, IIdeaQuery
    {
        public IdeaQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Idea>> GetAllAsync()
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Idea> GetByIdAsync()
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
}
