using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Queries;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.DTOs.RequestModels;
using Web.Api.Entities;
using Web.Api.Services.EmailService;

namespace Web.Api.Services.IdeaService
{
    public class IdeaService : IIdeaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Idea> _ideaRepo;
        private readonly IIdeaQuery _ideaQuery;
        private readonly IEmailService _emailService;
        private readonly UserManager<Entities.User> _userManager;
        private readonly IConfiguration _configuration;
        public IdeaService(IUnitOfWork unitOfWork, IIdeaQuery ideaQuery, IEmailService emailService, UserManager<Entities.User> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _ideaRepo = unitOfWork.GetBaseRepo<Idea>();
            _ideaQuery = ideaQuery;
            _emailService = emailService;
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<IEnumerable<Idea>> GetAllByAuthorAsync(string userName, Guid topicId)
        {
            try
            {
                return await _ideaQuery.GetAllByAuthorAsync(userName, topicId);
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

        public async Task<bool> SendEmailNotifyUserCreateIdea(Entities.Idea idea)
        {
            var ownerTopic = await _userManager.FindByIdAsync(idea.Topic.UserId.ToString());

            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string ideaDetailLink = _configuration.GetSection("Application:IdeaDetail").Value; //id topic + slug idea

            string logoUrl = "https://res.cloudinary.com/duasvwfje/image/upload/v1678291119/GoldenIdeaImg/GoldenIdea_prupeg.png";
            string html = "<table width=\"100%\" bgcolor=\"#f2f3f8\"\r\n  style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n  <tr>\r\n  <tr>\r\n      <td style=\"height:50px;\">&nbsp;</td>\r\n  </tr>\r\n    <td>\r\n      <table style=\"background-color: #f2f3f8; max-width:670px;  margin:auto auto;\" width=\"100%\" align=\"center\">\r\n        <tr>\r\n          <td>\r\n            <table width=\"95%\" border=\"0\" align=\"center\"\r\n              style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;\">\r\n              <tr>\r\n                <td style=\"text-align:center;\">\r\n                  <a title=\"logo\">\r\n                    <img width=\"25%\"\r\n                      src=\"" +
                logoUrl + "\"\r\n                      title=\"logo\" alt=\"logo\">\r\n                  </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"padding:0 35px;\">\r\n                  <h1 style=\"color:#1e1e2d; font-weight:500;font-size:32px;font-family:'Rubik',sans-serif;\">\r\n                    Your topic\r\n                   get a new idea!</h1>\r\n                  <span\r\n                    style=\"display:inline-block; vertical-align:middle; margin:10px 0 10px; border-bottom:1px solid #cecece; width:100px;\">";
            string html1 = "</span>\r\n                  <p style=\"color:#455056; font-size:1em;line-height:24px;\">\r\n                    Notify: Your topic get a new idea!\r\n                   </p>\r\n                  <a href=\"";
            string html2 = "\"\r\n                    style=\"background:#f6f872;text-decoration:none !important; font-weight:500; margin-top:35px; color:#000000;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Click here to see the idea</a>\r\n                    </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"height:40px;\">&nbsp;</td>\r\n              </tr>\r\n            </table>\r\n          </td>\r\n        <tr>\r\n          <td style=\"height:20px;\">&nbsp;</td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"text-align:center;\">\r\n            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\r\n              &copy; <strong>www.goldenidea.dungdoan.me.com</strong></p>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"height:50px;\">&nbsp;</td>\r\n        </tr>\r\n      </table>\r\n    </td>\r\n  </tr>\r\n</table>";
            string mainHtml = html1 + appDomain + ideaDetailLink + html2;
            SendEmailOptions option = new SendEmailOptions
            {
                ToName = ownerTopic.Name,
                ToEmail = ownerTopic.Email,
                Body = string.Format(html + mainHtml, idea.Slug),
                Subject = "[Golden Idea] Notify to your topic"
            };
            var result = await _emailService.SendEmailAsync(option.ToName, option.ToEmail, option.Subject, option.Body);
            return result;
        }
    }
}
