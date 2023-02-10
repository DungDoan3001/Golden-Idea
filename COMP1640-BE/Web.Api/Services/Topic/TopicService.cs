using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;

namespace Web.Api.Services.Topic
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Entities.Topic> _topicRepo;

        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _topicRepo = unitOfWork.GetBaseRepo<Entities.Topic>();
        }

        public async Task<IEnumerable<Entities.Topic>> GetAllAsync()
        {
            try
            {
                return await _topicRepo.All();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Topic> GetByIdAsync(Guid topicId)
        {
            try
            {
                return await _topicRepo.GetById(topicId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Topic> CreateAsync(Entities.Topic topic)
        {
            try
            {
                Entities.Topic createdTopic = _topicRepo.Add(topic);
                await _unitOfWork.CompleteAsync();
                return createdTopic;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Entities.Topic> UpdateAsync(Entities.Topic topic)
        {
            try
            {
                Entities.Topic updatedTopic = _topicRepo.Update(topic);
                await _unitOfWork.CompleteAsync();
                return updatedTopic;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid topicId)
        {
            try
            {
                bool isDelete = _topicRepo.Delete(topicId);
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

        public async Task<IEnumerable<Entities.Topic>> GetByNameAsync(string name)
        {
            IEnumerable<Entities.Topic> topics = await _topicRepo.Find(x => x.Name == name);
            return topics;
        }
    }
}
