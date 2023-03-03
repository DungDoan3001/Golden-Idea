using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Services.IdeaService
{
    public interface IIdeaService
    {
        Task<IEnumerable<Idea>> GetAllAsync();
        Task<Idea> CreateAsync(Idea idea);
    }
}