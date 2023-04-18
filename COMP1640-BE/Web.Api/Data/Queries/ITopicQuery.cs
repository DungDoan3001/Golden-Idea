using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface ITopicQuery
    {
        Task<List<Topic>> GetAllAsync();
        Task<List<Topic>> GetAllByUserName(string userName);
        Task<Topic> GetByIdAsync(Guid id);
        Task<List<Topic>> GetByUserId(Guid id);
    }
}