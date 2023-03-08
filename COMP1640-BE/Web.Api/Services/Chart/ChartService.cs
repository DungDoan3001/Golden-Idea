using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.Chart
{
    public class ChartService : IChartService
    {
        private readonly UserManager<Entities.User> _userManager;
        protected AppDbContext _context;
        private readonly IMapper _mapper;
        public ChartService(UserManager<Entities.User> userManager, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ContributorResponseModel>> GetContributorByDepart()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                List<ContributorResponseModel> result = new List<ContributorResponseModel>();
                foreach (var department in departments)
                {
                    ContributorResponseModel contributors = new ContributorResponseModel();
                    contributors.DepartmentName = department.Name;
                    var usersInDepartment = await _context.Users
                        .Where(x => x.DepartmentId == department.Id)
                        .AsNoTracking()
                        .ToListAsync();
                    contributors.TotalUser = usersInDepartment.Count();
                    foreach (var user in usersInDepartment)
                    {
                        var checkUserPostedIdea = await _context.Ideas
                        .Where(x => x.UserId == user.Id)
                        .AsNoTracking()
                        .AnyAsync();
                        if (checkUserPostedIdea)
                        {
                            contributors.Contributor = contributors.Contributor + 1;
                        }
                    }
                    contributors.NonContributor = contributors.TotalUser - contributors.Contributor;
                    result.Add(contributors);
                }               
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<NumOfIdeaAnonyByDepartment>> GetNumOfIdeaAnonyAndNoCommentByDepart()
        {
            try
            {
                var ideas = await _context.Ideas
                    .Include(x => x.Topic)
                    .ThenInclude(x => x.User)
                    .ToListAsync();
                var departments = await _context.Departments.ToListAsync();
                List<NumOfIdeaAnonyByDepartment> result = new List<NumOfIdeaAnonyByDepartment>();
                foreach(var department in departments)
                {
                    NumOfIdeaAnonyByDepartment data = new NumOfIdeaAnonyByDepartment();
                    int countIdeaNoComment = 0;
                    int countIdeaAnonymous = 0;
                    data.DepartmentName = department.Name;
                    foreach (var idea in ideas)
                    {            
                        if(idea.User.DepartmentId == department.Id)
                        {
                            if (await _context.Comments.Where(x => x.IdeaId == idea.Id).AsNoTracking().CountAsync() == 0)
                            {
                                countIdeaNoComment++;
                            }
                            if(idea.IsAnonymous)
                            {
                                countIdeaAnonymous++;
                            }
                            data.IdeaNoComment = countIdeaNoComment;
                            data.IdeaAnonymous = countIdeaAnonymous;
                        }
                    }
                    result.Add(data);
                }  
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<NumOfCommentResponseModel>> GetNumOfCommentByDepart()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();
            var comments = await _context.Comments
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync();
            List<NumOfCommentResponseModel> result = new List<NumOfCommentResponseModel>();
            foreach(var department in departments)
            {
                NumOfCommentResponseModel data = new NumOfCommentResponseModel();
                int countCommentAnonymous = 0;
                int countCommentNonAnonymous = 0;
                data.DepartmentName = department.Name;
                foreach(var comment in comments)
                {
                    
                    if(comment.User.DepartmentId == department.Id)
                    {
                        if(comment.IsAnonymous == true)
                        {
                            countCommentAnonymous++;
                        }
                        if(comment.IsAnonymous == false)
                        {
                            countCommentNonAnonymous++;
                        }
                    }
                }
                data.CommentAnonymous = countCommentAnonymous;
                data.CommentNonAnonymous = countCommentNonAnonymous;
                result.Add(data);
            }
            return result;
        }
    }

}
