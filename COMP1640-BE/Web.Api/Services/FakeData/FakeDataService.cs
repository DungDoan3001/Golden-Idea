using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Slugify;
using Web.Api.Data.Context;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;
using Web.Api.Helpers;

namespace Web.Api.Services.FakeData
{
    public class FakeDataService : IFakeDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Idea> _ideaRepo;
        private readonly IAppDbContext _context;
        private SlugHelper _slugHelper = new SlugHelper();
        private RandomDateTime randomDateTime = new RandomDateTime();
        public FakeDataService(IUnitOfWork unitOfWork, IAppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _ideaRepo = unitOfWork.GetBaseRepo<Idea>();
            _context = context;
        }

        public async Task<bool> CreateFakeIdeaData(int numberOfIdeaToCreate)
        {
            try
            {
                Guid uniquePerFake = Guid.NewGuid();
                string uniqueShortId = uniquePerFake.ToString().Split('-')[0];
                string title = "This Idea is fake only for testing - " + uniqueShortId;
                string content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, " +
                    "remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. " +
                    "\n It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using " +
                    "'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their " +
                    "infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).";
                string image = "https://res.cloudinary.com/duasvwfje/image/upload/v1678633475/GoldenIdeaImg/ramdomtwitter_Mesa_20de_20trabajo_201_fpggj7.png";

                // Get the lastest list of data
                IEnumerable<Entities.User> users = await _context.Users.ToListAsync();
                IEnumerable<Entities.Topic> topics = await _context.Topics.ToListAsync();
                IEnumerable<Entities.Category> categories = await _context.Categories.ToListAsync();


                IEnumerable<Guid> userIdLs = users.Select(x => x.Id);
                IEnumerable<Guid> topicIdLs = topics.Select(x => x.Id);
                IEnumerable<Guid> categoryIdLs = categories.Select(x => x.Id);

                List<Idea> fakeIdeas = new List<Idea>();
                //Begin to fake Data
                for (int i = 1; i <= numberOfIdeaToCreate; i++)
                {
                    Guid userId = RandomSelect.GetRandom(userIdLs, 1).FirstOrDefault();
                    Guid categoryId = RandomSelect.GetRandom(categoryIdLs, 1).FirstOrDefault();
                    Guid topicId = RandomSelect.GetRandom(topicIdLs, 1).FirstOrDefault();

                    Entities.Topic topic = topics.Where(x => x.Id == topicId).FirstOrDefault();

                    // Reset random date gen to match the topic ClosureDate
                    randomDateTime.range = (topic.ClosureDate - randomDateTime.start).Days;
                    // Get the random date
                    var randomDate = randomDateTime.Next();

                    Idea idea = new Idea
                    {
                        Title = title,
                        Content = content,
                        Image = image,
                        Slug = _slugHelper.GenerateSlug(title) + "-" + i,
                        IsAnonymous = true,
                        UserId = userId,
                        CategoryId = categoryId,
                        TopicId = topicId,
                        CreatedAt = randomDate,
                        LastUpdate = randomDate,
                        IsFakeData = true,
                    };
                    fakeIdeas.Add(idea);
                }
                _ideaRepo.AddRange(fakeIdeas);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFakeDataAsync()
        {
            try
            {
                IEnumerable<Idea> fakeIdeas = await _context.Ideas.Where(x => x.IsFakeData).ToListAsync();
                _ideaRepo.DeleteRange(fakeIdeas);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
