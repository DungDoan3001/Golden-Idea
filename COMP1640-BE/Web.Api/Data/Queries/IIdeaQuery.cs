using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface IIdeaQuery
    {
        Task<List<Idea>> GetAllAsync();
        Task<Idea> GetByIdAsync();
    }
}