using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Services.Topic
{
    public interface ITopicService
    {
        Task<Entities.Topic> CreateAsync(Entities.Topic topic);
        Task<bool> DeleteAsync(Guid topicId);
        Task<IEnumerable<Entities.Topic>> GetAllAsync();
        Task<Entities.Topic> GetByIdAsync(Guid topicId);
        Task<IEnumerable<Entities.Topic>> GetByNameAsync(string name);
        Task<Entities.Topic> UpdateAsync(Entities.Topic topic);
    }
}