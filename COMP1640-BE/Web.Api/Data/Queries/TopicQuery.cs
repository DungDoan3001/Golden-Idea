using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
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
                .ToListAsync();
        }

        public async Task<Topic> GetByIdAsync(Guid id)
        {
            return await dbSet
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
