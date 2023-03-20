using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        protected IAppDbContext _context;
        private readonly IMapper _mapper;
        public ChartService(IAppDbContext context, IMapper mapper)
        {
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
                    .AsSplitQuery()
                    .AsNoTracking()
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
                        if(idea.Topic.User.DepartmentId == department.Id)
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
            try
            {
                var departments = await _context.Departments.AsNoTracking().ToListAsync();
                var comments = await _context.Comments
                    .Include(x => x.User)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync();
                List<NumOfCommentResponseModel> result = new List<NumOfCommentResponseModel>();
                foreach (var department in departments)
                {
                    NumOfCommentResponseModel data = new NumOfCommentResponseModel();
                    int countCommentAnonymous = 0;
                    int countCommentNonAnonymous = 0;
                    data.DepartmentName = department.Name;
                    foreach (var comment in comments)
                    {

                        if (comment.User.DepartmentId == department.Id)
                        {
                            if (comment.IsAnonymous == true)
                            {
                                countCommentAnonymous++;
                            }
                            if (comment.IsAnonymous == false)
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TotalIdeaOfDepartmentsResponseModel>> GetTotalIdeaOfEachDepartment()
        {
            try
            {
                var departments = await _context.Departments
                                .Include(x => x.Users).ThenInclude(x => x.Topics).ThenInclude(x => x.Ideas)
                                .AsNoTracking()
                                .AsSplitQuery()
                                .ToListAsync();
                List<TotalIdeaOfDepartmentsResponseModel> result = new List<TotalIdeaOfDepartmentsResponseModel>();

                foreach (var department in departments)
                {
                    int totalIdea = 0;
                    foreach (var user in department.Users)
                    {
                        foreach (var topic in user.Topics)
                        {
                            totalIdea += topic.Ideas.Count;
                        };
                    }
                    TotalIdeaOfDepartmentsResponseModel data = new TotalIdeaOfDepartmentsResponseModel
                    {
                        DepartmentName = department.Name,
                        TotalIdea = totalIdea,
                    };
                    result.Add(data);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TotalStaffAndIdeaAndTopicAndCommentResponseModel> GetTotalOfStaffAndIdeaAndTopicAndCommment()
        {
            try
            {
                int totalStaff = await _context.Users.CountAsync();
                int totalIdea = await _context.Ideas.CountAsync();
                int totalTopic = await _context.Topics.CountAsync();
                int totalComment = await _context.Comments.CountAsync();

                TotalStaffAndIdeaAndTopicAndCommentResponseModel data = new TotalStaffAndIdeaAndTopicAndCommentResponseModel
                {
                    TotalStaff = totalStaff,
                    TotalIdea = totalIdea,
                    TotalTopic = totalTopic,
                    TotalComment = totalComment
                };

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PercentageOfIdeaForEachDepartment>> GetPercentageOfIdeaForEachDepartments()
        {
            try
            {
                var totalIdeaOfEachDepartment = await GetTotalIdeaOfEachDepartment();
                int totalIdea = 0;
                totalIdeaOfEachDepartment.ForEach(x =>
                {
                    totalIdea += x.TotalIdea;
                });

                List<PercentageOfIdeaForEachDepartment> result = new List<PercentageOfIdeaForEachDepartment>();
                if(totalIdea > 0)
                {
                    decimal totalPercentage = 0;

                    for (int i = 0; i < totalIdeaOfEachDepartment.Count; i++)
                    {
                        var department = totalIdeaOfEachDepartment[i];
                        decimal realPercentage = (decimal)department.TotalIdea * 100 / totalIdea;
                        Console.WriteLine(realPercentage.ToString());
                        decimal ceilPercentage = Math.Floor(realPercentage);
                        totalPercentage += ceilPercentage;
                        PercentageOfIdeaForEachDepartment data = new PercentageOfIdeaForEachDepartment
                        {
                            DepartmentName = department.DepartmentName,
                            Percentage = ceilPercentage,
                        };
                        result.Add(data);
                    }

                    if (100 - totalPercentage != 0)
                    {
                        var surplus = 100 - totalPercentage;
                        var smallestPercentage = result.Where(x => x.Percentage > 0).OrderBy(x => x.Percentage).Take(1).SingleOrDefault();
                        smallestPercentage.Percentage += surplus;
                    }
                    return result;
                } else { return result; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<IdeaForChartResponseModel>> GetIdeasForChart()
        {
            try
            {
                var data = await _context.Ideas
                .Include(x => x.User)
                .Include(x => x.Comments)
                .Include(x => x.Reactions)
                .Include(x => x.Views)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

                List<IdeaForChartResponseModel> result = _mapper.Map<List<IdeaForChartResponseModel>>(data);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<DailyReportResponseModel>> GetDailyReportInThreeMonths()
        {
            try
            {
                DateTime threeMonthPrevious = DateTime.UtcNow.AddMonths(-3);
                int totalDays = 0;
                for (int i = 0; i < 3; i++)
                {
                    totalDays = totalDays + DateTime.DaysInMonth(DateTime.UtcNow.AddMonths(-i).Year, DateTime.UtcNow.AddMonths(-i).Month);
                }
                List<DailyReportResponseModel> result = new List<DailyReportResponseModel>();
                var totalIdea = await _context.Ideas
                    .Where(x => x.CreatedAt <= DateTime.UtcNow && x.CreatedAt >= threeMonthPrevious)
                    .AsNoTracking()
                    .ToListAsync();
                for (int i = 0; i <= totalDays; i++)
                {
                    DailyReportResponseModel data = new DailyReportResponseModel();
                    DateTime date = threeMonthPrevious;
                    int countIdea = 0;
                    int countComment = 0; 
                    foreach(var idea in totalIdea)
                    {
                        if(String.Equals(idea.CreatedAt.ToString("yyyy-MM-dd"), date.AddDays(i).ToString("yyyy-MM-dd")))
                        {
                            countIdea++;
                        }
                    }
                    var totalComment = await _context.Comments.ToListAsync();
                    foreach (var comment in totalComment)
                    {
                        if (String.Equals(comment.CreatedDate.ToString("yyyy-MM-dd"), date.AddDays(i).ToString("yyyy-MM-dd")))
                        {
                            countComment++;
                        }
                    }
                    data.Date = date.AddDays(i).ToString("yyyy-MM-dd");
                    data.TotalComment = countComment;
                    data.TotalIdea = countIdea;
                    result.Add(data);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
