using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Context;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public class TopicQuery : BaseQuery<Topic>, ITopicQuery
    {
        public TopicQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Topic>> GetAllAsync()
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Ideas)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Topic> GetByIdAsync(Guid id)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Ideas)
                .Where(x => x.Id == id)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }

        public async Task<List<Topic>> GetByUserId(Guid id)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Ideas)
                .Where(x => x.UserId == id)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
