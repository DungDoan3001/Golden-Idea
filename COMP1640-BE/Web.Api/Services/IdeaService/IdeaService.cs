using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Idea>> GetAllByAuthorAsync(string userName)
        {
            try
            {
                return await _ideaQuery.GetAllByAuthorAsync(userName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Idea>> GetAllByTopicAsync(Guid topicId)
        {
            try
            {
                return await _ideaQuery.GetAllByTopicAsync(topicId);
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

        public async Task<Idea> UpdateAsync(Idea idea)
        {
            try
            {
                Idea updatedIdea = _ideaRepo.Update(idea);
                await _unitOfWork.CompleteAsync();
                return updatedIdea;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid ideaId)
        {
            try
            {
                bool isDelete = _ideaRepo.Delete(ideaId);
                if (isDelete)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckSlugExistedAsync(string slug)
        {
            try
            {
                return await _ideaQuery.CheckSlugExistedAsync(slug);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckIdeaExisted(Guid id)
        {
            try
            {
                return await _ideaQuery.CheckIdeaExistedAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckExistedImageContainDuplicateAsync(string image)
        {
            var files = await _ideaRepo.Find(x => x.Image == image);
            if (files.Count() > 1)
                return true;
            return false;
        }
        public async Task<List<Idea>> SearchByTitle(string searchTerm)
        {
            return await _ideaQuery.Search(searchTerm);
        }
    }
}
