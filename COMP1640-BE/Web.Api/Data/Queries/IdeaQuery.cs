using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.Entities;

namespace Web.Api.Data.Queries
{
    public class IdeaQuery : BaseQuery<Idea>, IIdeaQuery
    {
        public IdeaQuery(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Idea>> GetAllAsync()
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Idea>> GetAllByAuthorAsync(string userName, Guid topicId)
        {
            if(!(topicId == Guid.Empty))
            {
                return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .Where(x => x.User.UserName.Trim().ToLower() == userName.Trim().ToLower() && x.Topic.Id == topicId)
                .AsSplitQuery()
                .ToListAsync();
            } else
            {
                return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .Where(x => x.User.UserName.Trim().ToLower() == userName.Trim().ToLower())
                .AsSplitQuery()
                .ToListAsync();
            }
        }

        public async Task<List<Idea>> GetAllByUserNameForTopicServiceAsync(string userName)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic).ThenInclude(x => x.User)
                .Where(x => x.User.UserName.Trim().ToLower() == userName.Trim().ToLower())
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Idea>> GetAllByTopicAsync(Guid topicId)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .Where(x => x.TopicId == topicId)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Idea> GetByIdAsync(Guid id)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .Where(x => x.Id == id)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }

        public async Task<Idea> GetBySlugAsync(string slug)
        {
            return await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .Where(x => x.Slug == slug)
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }

        public async Task<bool> CheckSlugExistedAsync(string slug)
        {
            return await dbSet.AnyAsync(x => x.Slug == slug);
        }

        public async Task<bool> CheckIdeaExistedAsync(Guid id)
        {
            return await dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Idea>> Search(string searchTerm)
        {
            var ideas = await dbSet
                .Include(x => x.User)
                .Include(x => x.Topic)
                .Include(x => x.Category)
                .Include(x => x.Files)
                .Include(x => x.Views)
                .Include(x => x.Reactions)
                .AsSplitQuery()
                .ToListAsync();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return ideas.Take(10).ToList();
            }
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return ideas
                .Where(x => x.Title.ToLower().Contains(lowerCaseTerm))
                .OrderBy(x => x.Title)
                .ToList();
        }
    }
}
