using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Services.IdeaService
{
    public interface IIdeaService
    {
        Task<IEnumerable<Idea>> GetAllAsync();
        Task<Idea> GetByIdAsync(Guid id);
        Task<Idea> GetBySlugAsync(string slug);
        Task<Idea> CreateAsync(Idea idea);
        Task<Idea> UpdateAsync(Idea idea);
        Task<bool> CheckSlugExistedAsync(string slug);
        Task<bool> CheckIdeaExisted(Guid id);
        Task<bool> CheckExistedImageContainDuplicateAsync(string image);
    }
}