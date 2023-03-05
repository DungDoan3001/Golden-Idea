using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Data.Queries;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;

namespace Web.Api.Services.IdeaService
{
    public class IdeaService : IIdeaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Idea> _ideaRepo;
        private readonly IIdeaQuery _ideaQuery;

        public IdeaService(IUnitOfWork unitOfWork, IIdeaQuery ideaQuery)
        {
            _unitOfWork = unitOfWork;
            _ideaRepo = unitOfWork.GetBaseRepo<Idea>();
            _ideaQuery = ideaQuery;
        }

        public async Task<IEnumerable<Idea>> GetAllAsync()
        {
            try
            {
                return await _ideaQuery.GetAllAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Idea> GetByIdAsync(Guid id)
        {
            try
            {
                return await _ideaQuery.GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Idea> GetBySlugAsync(string slug)
        {
            try
            {
                return await _ideaQuery.GetBySlugAsync(slug);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Idea> CreateAsync(Idea idea)
        {
            try
            {
                Idea createdIdea = _ideaRepo.Add(idea);
                await _unitOfWork.CompleteAsync();
                return createdIdea;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckSlugExitAsync(string slug)
        {
            try
            {
                return await _ideaQuery.CheckSlugExitAsync(slug);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
