﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public interface IIdeaQuery
    {
        Task<List<Idea>> GetAllAsync();
        Task<List<Idea>> GetAllByAuthorAsync(string userName, Guid topicId);
        Task<List<Idea>> GetAllByTopicAsync(Guid topicId);
        Task<Idea> GetByIdAsync(Guid id);
        Task<Idea> GetBySlugAsync(string slug);
        Task<bool> CheckSlugExistedAsync(string slug);
        Task<bool> CheckIdeaExistedAsync(Guid id);
        Task<List<Idea>> Search(string searchTerm);
        Task<List<Idea>> GetAllByUserNameForTopicServiceAsync(string userName);
    }
}