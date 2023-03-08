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

        public async Task<List<ContributorResponseModel>> GetContributor()
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
                        .ToListAsync();
                    contributors.TotalUser = usersInDepartment.Count();
                    foreach (var user in usersInDepartment)
                    {
                        var checkUserPostedIdea = await _context.Ideas
                        .Where(x => x.UserId == user.Id)
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
        public async Task<List<NumOfIdeaAnonyByDepartment>> GetNumOfIdeaAnonyByDepartment()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                foreach (var department in departments)
                {
                    var ideasOfDepartment = await _context.Ideas
                        .Include(x => x.Topic)
                        .ThenInclude(x => x.User)
                        .ThenInclude(x => x.DepartmentId)
                        .ToListAsync();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<List<TotalCommentOfDepartmentsResponseModel>> GetTotalCommentOfDepartments()
        //{

        //}
    }

}
