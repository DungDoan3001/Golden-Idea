using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Data.Context;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Web.Api.DTOs.ResponseModels;
using Web.Api.DTOs.RequestModels;
using Microsoft.AspNetCore.Identity;
using Web.Api.Data.Queries;
using Microsoft.Extensions.Configuration;
using Web.Api.Services.EmailService;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Configuration;

namespace Web.Api.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Entities.Comment> _commentRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Entities.User> _userManager;
        private readonly ICommentQuery _commentQuery;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;

        public CommentService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper, UserManager<Entities.User> userManager, ICommentQuery commentQuery, IConfiguration configuration, IEmailService emailService, IMemoryCache cache, CacheKey cacheKey)
        {
            _unitOfWork = unitOfWork;
            _commentRepo = unitOfWork.GetBaseRepo<Entities.Comment>();
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _commentQuery = commentQuery;
            _configuration = configuration;
            _emailService = emailService;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public async Task<CommentResponseModel> Create(CommentRequestModel comment)
        {
            try
            {
                var idea = await _context.Ideas.FindAsync(comment.IdeaId);
                if (idea == null)
                {
                    throw new Exception("Can not find idea!");
                }
                var user = await _userManager.FindByNameAsync(comment.Username);
                if (user == null)
                {
                    throw new Exception("Can not find the user!");
                }
                var addComment = _commentRepo.Add(new Entities.Comment
                {
                    UserId = user.Id,
                    IdeaId = comment.IdeaId,
                    Content = comment.Content,
                    IsAnonymous = comment.IsAnonymous
                });
                await _unitOfWork.CompleteAsync();
                var result = _mapper.Map<CommentResponseModel>(addComment);
                result.Username = user.UserName;
                result.Avatar = user.Avatar;
                //Send email to owner idea
                if(!await SendEmailNotifyUserCommentOnIdea(idea, addComment.UserId))
                {
                    throw new Exception("Send an email to the owner idea is failed!");
                }
                // Delete cache for Exception Report Chart Ideas + Comments
                await Task.Run(() =>
                {
                    _cache.Remove(_cacheKey.NumOfIdeaAnonyAndNoCommentByDepartCacheKey);
                    _cache.Remove(_cacheKey.NumOfCommentByDepartCacheKey);
                    // Delete cache for chart TotalStaffAndIdeaAndCommentAndTopic
                    _cache.Remove(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey);
                    // Delete cache for GetDailyReportInThreeMonths chart
                    _cache.Remove(_cacheKey.DailyReportInThreeMonthsCacheKey);
                });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<CommentResponseModel>> GetAllCommentOfIdea(Guid ideaId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(x => x.IdeaId == ideaId)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync();
                var result = _mapper.Map<List<CommentResponseModel>>(comments);
                //Get username
                int i = 0;
                foreach(var item in comments)
                {
                    var user = await _userManager.FindByIdAsync(item.UserId.ToString());
                    if(user == null)
                    {
                        throw new Exception("Can not find the user!");
                    }
                    if(i < result.Count)
                    {
                        result[i].Username = user.UserName;
                        result[i].Avatar = user.Avatar;
                        i++;
                    }
                }           
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteByIdeaAsync(Guid ideaId)
        {
            try
            {
                var comments = await _commentQuery.GetAllByIdeaAsync(ideaId);
                if (comments.Any())
                {
                    _commentRepo.DeleteRange(comments);
                    await _unitOfWork.CompleteAsync();
                    await Task.Run(() =>
                    {
                        // Delete cache for GetDailyReportInThreeMonths chart
                        _cache.Remove(_cacheKey.NumOfCommentByDepartCacheKey);
                    });
                    return true;
                } else { return false; }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SendEmailNotifyUserCommentOnIdea(Entities.Idea idea, Guid userIdComment)
        {
            var userComment = await _userManager.FindByIdAsync(userIdComment.ToString());
            var ownerIdea = await _userManager.FindByIdAsync(idea.UserId.ToString());

            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string ideaDetailLink = _configuration.GetSection("Application:IdeaDetail").Value; //id topic + slug idea

            string logoUrl = "https://res.cloudinary.com/duasvwfje/image/upload/v1678291119/GoldenIdeaImg/GoldenIdea_prupeg.png";
            string html = "<table width=\"100%\" bgcolor=\"#f2f3f8\"\r\n  style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n  <tr>\r\n  <tr>\r\n      <td style=\"height:50px;\">&nbsp;</td>\r\n  </tr>\r\n    <td>\r\n      <table style=\"background-color: #f2f3f8; max-width:670px;  margin:auto auto;\" width=\"100%\" align=\"center\">\r\n        <tr>\r\n          <td>\r\n            <table width=\"95%\" border=\"0\" align=\"center\"\r\n              style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;\">\r\n              <tr>\r\n                <td style=\"text-align:center;\">\r\n                  <a title=\"logo\">\r\n                    <img width=\"25%\"\r\n                      src=\"" +
                logoUrl + "\"\r\n                      title=\"logo\" alt=\"logo\">\r\n                  </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"padding:0 35px;\">\r\n                  <h1 style=\"color:#1e1e2d; font-weight:500;font-size:32px;font-family:'Rubik',sans-serif;\">\r\n                    Your idea\r\n                   get a new comment from " +
                userComment.UserName + "!</h1>\r\n                  <span\r\n                    style=\"display:inline-block; vertical-align:middle; margin:10px 0 10px; border-bottom:1px solid #cecece; width:100px;\">";
            string html1 = "</span>\r\n                  <p style=\"color:#455056; font-size:1em;line-height:24px;\">\r\n                    The title of your idea: " +
                idea.Title +"!\r\n                   </p>\r\n                  <a href=\"";
            string html2 = "\"\r\n                    style=\"background:#f6f872;text-decoration:none !important; font-weight:500; margin-top:35px; color:#000000;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Click here to see the idea</a>\r\n                    </a>\r\n                </td>\r\n              </tr>\r\n              <tr>\r\n                <td style=\"height:40px;\">&nbsp;</td>\r\n              </tr>\r\n            </table>\r\n          </td>\r\n        <tr>\r\n          <td style=\"height:20px;\">&nbsp;</td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"text-align:center;\">\r\n            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\r\n              &copy; <strong>www.goldenidea.dungdoan.me.com</strong></p>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"height:50px;\">&nbsp;</td>\r\n        </tr>\r\n      </table>\r\n    </td>\r\n  </tr>\r\n</table>";
            string mainHtml = html1 + appDomain + ideaDetailLink + html2;
            SendEmailOptions option = new SendEmailOptions
            {
                ToName = ownerIdea.Name,
                ToEmail = ownerIdea.Email,
                Body = string.Format(html + mainHtml, idea.Slug),
                Subject = "[Golden Idea] Notify to your idea"
            };
            var result = await _emailService.SendEmailAsync(option.ToName, option.ToEmail, option.Subject, option.Body);
            return result;
        }
    }
}
