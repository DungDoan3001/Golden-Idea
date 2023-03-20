using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface ICommentQuery
    {
        Task<List<Comment>> GetAllByIdeaAsync(Guid ideaId);
    }
}