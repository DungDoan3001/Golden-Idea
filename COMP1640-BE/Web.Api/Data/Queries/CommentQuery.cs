using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Context;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public class CommentQuery : BaseQuery<Comment>, ICommentQuery
    {
        public CommentQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Comment>> GetAllByIdeaAsync(Guid ideaId)
        {
            return await dbSet
                .Where(x => x.IdeaId == ideaId)
                .ToListAsync();
        }
    }
}
