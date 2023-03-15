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


        private List<string> titles = new List<string> { "The School Should Prioritize Student Safety with Adequate Security Measures",
    "The School Should Invest in Upgraded Classroom Technology for Modern Learning",
    "The School Should Provide Accessible Restrooms for Students with Disabilities",
    "The School Should Have a Dedicated Counseling Center for Student Mental Health",
    "The School Should Offer Healthy and Nutritious Meal Options in the Cafeteria",
    "The School Should Provide Accessible Outdoor Spaces for Learning and Play",
    "The School Should Have a Dedicated Health Clinic for Student Medical Needs",
    "The School Should Offer Comprehensive Sexual Education Programs",
    "The School Should Provide Adequate Storage Space for Student Belongings",
    "The School Should Have a Well-Stocked Library for Learning Resources",
    "The School Should Offer Diverse Extracurricular Activities for Student Engagement",
    "The School Should Provide Accessible Technology for Students with Disabilities",
    "The School Should Have a Dedicated Parent Resource Center for Family Support",
    "The School Should Prioritize Inclusive Design for Students with Different Learning Styles",
    "The School Should Offer Accessible Transportation Options for Students",
    "The School Should Provide Adequate Student Parking for Commuters",
    "The School Should Have a Dedicated Media Center for Digital Learning",
    "The School Should Offer Comprehensive Language Learning Programs",
    "The School Should Provide Adequate Funding for Arts and Music Programs",
    "The School Should Offer Comprehensive Programs for Students with Special Needs",
    "The School Should Prioritize Energy Efficiency with Sustainable Building Practices",
    "The School Should Have a Dedicated Science Lab for Hands-On Learning",
    "The School Should Offer Comprehensive Financial Aid Programs for Low-Income Students",
    "The School Should Provide Accessible Sports and Athletics Facilities",
    "The School Should Offer Comprehensive Career Counseling Services",
    "The School Should Prioritize Diversity and Inclusion in Staff Hiring Practices",
    "The School Should Provide Accessible Technology for English Language Learners",
    "The School Should Have a Dedicated Technology Repair Center for Student Devices",
    "The School Should Offer Comprehensive Driver Education Programs",
    "The School Should Prioritize Mental Health Support for Staff Members",
    "The School Should Provide Adequate Storage Space for Athletic Equipment",
    "The School Should Offer Comprehensive Summer Enrichment Programs",
    "The School Should Prioritize Campus Sustainability with Recycling and Composting Programs",
    "The School Should Have a Dedicated Student Leadership Center for Developing Leadership Skills",
    "The School Should Provide Accessible Educational Materials for Students with Visual Impairments",
    "The School Should Offer Comprehensive STEM Programs for Science and Technology Education",
    "The School Should Prioritize Teacher Professional Development Opportunities",
    "The School Should Provide Adequate Funding for Theatre and Performing Arts Programs",
    "The School Should Offer Comprehensive Programs for Gifted and Talented Students",
    "The School Should Prioritize Accessible Design for Students with Physical Disabilities",
    "The School Should Provide Accessible Technology for Students with Hearing Impairments",
    "The School Should Offer Comprehensive Health and Wellness Programs for Students",
    "The School Should Prioritize Inclusive Design for Students with Autism Spectrum Disorders",
    "The School Should Provide Adequate Funding for Debate and Public Speaking Programs",
    "The School Should Offer Comprehensive Programs for English Language Learners",
    "The School Should Prioritize Student Mental Health with Regular Counseling Services",
    "The School Should Provide Adequate Funding for Robotics and Technology Programs",
    "The School Should Offer Comprehensive Programs for Students with Learning Disabilities",
    "The School Should Prioritize Campus Safety with Regular Emergency Preparedness Drills",
    "The School Should Provide Adequate Funding for Visual Arts Programs",
    "The School Should Offer Comprehensive Programs for Students with Behavioral Disorders",
    "The School Should Prioritize Inclusive Design for Students with Cognitive Disabilities",
    "The School Should Provide Accessible Technology for Students with Mobility Impairments",
    "The School Should Offer Comprehensive Programs for Community Service and Volunteerism",
    "The School Should Prioritize Student Mental Health with Regular Meditation and Mindfulness Practices",
    "The School Should Provide Adequate Funding for Debate and Speech Programs",
    "The School Should Offer Comprehensive Programs for Gifted and Talented Students in the Arts",
    "The School Should Prioritize Accessibility with Regular Campus Accessibility Audits",
    "The School Should Provide Adequate Funding for Music Programs",
    "The School Should Offer Comprehensive Programs for Students with Emotional and Behavioral Disorders",
    "The School Should Prioritize Inclusive Design for Students with Language Processing Disorders",
    "The School Should Provide Accessible Technology for Students with Visual Impairments",
    "The School Should Offer Comprehensive Programs for Leadership Development",
    "The School Should Prioritize Student Mental Health with Regular Therapy Sessions",
    "The School Should Provide Adequate Funding for STEM Programs for Girls",
    "The School Should Offer Comprehensive Programs for Students with Physical Disabilities" 
        };
        private List<string> images = new List<string>
        {
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678633475/GoldenIdeaImg/ramdomtwitter_Mesa_20de_20trabajo_201_fpggj7.png",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784316/GoldenIdeaImg/1b9c5edf895e27b842ce49c73d48a385_ab12jb.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784340/GoldenIdeaImg/9b04b3de845880bed61c2b3411062869_nlooo0.jpg",
            "https://i.pinimg.com/564x/f5/c6/9f/f5c69f1c388ea51ab3f9739f620412ee.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784382/GoldenIdeaImg/44b22579ff75e2bff2e651b6184a9b5b_ag6xjb.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784400/GoldenIdeaImg/4d14adfe064eb5ab426b0d934634b775_xxyczi.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784421/GoldenIdeaImg/24f5a325396a398ce004cdcf0379fe44_fxdnbu.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784461/GoldenIdeaImg/b91a2b58a66acb0f09fc5f3fb1566ffd_gtl8lp.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784491/GoldenIdeaImg/393fd89e68af0bca05250bdd28fde98f_e5pio3.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784508/GoldenIdeaImg/58c10d15dc253f8a0e468b8c345e78e8_txllid.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784530/GoldenIdeaImg/37bed85550d6061138dead3040beacf2_mbo7xn.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784550/GoldenIdeaImg/200050daa2c93a67395df795a0d47d81_iziupz.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784589/GoldenIdeaImg/078695b0839d295241e9ca533bfb649b_pk9h4a.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784604/GoldenIdeaImg/111c3977e2432749c52aba0f6b907a7f_cajlmt.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784620/GoldenIdeaImg/1b8c669f290e06b645b83d84646cf1db_sogxbt.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784637/GoldenIdeaImg/b513f2c94d3111140a9143770dd923df_xqirnl.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784665/GoldenIdeaImg/7bf7815a4165392ef10896d4ae6d4ed4_hzto3y.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784682/GoldenIdeaImg/59c7211706743d774b73c111bddf5d44_dipco9.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784697/GoldenIdeaImg/189f84bd433f14b03f11052eb63f1449_sep3gf.jpg",
            "https://res.cloudinary.com/duasvwfje/image/upload/v1678784714/GoldenIdeaImg/05c2602f3a538d045aff850eda471295_jxzpq4.jpg",
        };
        private string content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, " +
                    "remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. " +
                    "\n It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using " +
                    "'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their " +
                    "infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).";
        





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
                    string title = RandomSelect.GetRandom(this.titles, 1).FirstOrDefault() + " - " + uniqueShortId;
                    string image = RandomSelect.GetRandom(this.images, 1).FirstOrDefault();
                    // Reset random date gen to match the topic ClosureDate
                    randomDateTime.range = (topic.ClosureDate - randomDateTime.start).Days;
                    // Get the random date
                    var randomDate = randomDateTime.Next();

                    Idea idea = new Idea
                    {
                        Title = title,
                        Content = this.content,
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
