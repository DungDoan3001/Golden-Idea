using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface IIdeaQuery
    {
        Task<List<Idea>> GetAllAsync();
        Task<List<Idea>> GetAllByAuthorAsync(Guid userId);
        Task<Idea> GetByIdAsync(Guid id);
        Task<Idea> GetBySlugAsync(string slug);
        Task<bool> CheckSlugExistedAsync(string slug);
        Task<bool> CheckIdeaExistedAsync(Guid id);
        Task<List<Idea>> Search(string searchTerm);
    }
}