using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Configuration;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities.Configuration;
using Web.Api.Extensions;
using Web.Api.Services.Chart;

namespace Web.Api.Controllers
{
    [Route("api/charts")]
    [Authorize]
    [ApiController]
    public class ChartController : ControllerBase
    {
        public readonly IChartService _chartService;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;
        public ChartController(IChartService chartService, CacheKey cacheKey, IMemoryCache cache)
        {
            _chartService = chartService;
            _cacheKey = cacheKey;
            _cache = cache;
        }
        /// <summary>
        /// Get all needed information for Contributors Chart.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("contributors-by-department")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<ContributorResponseModel>>> GetContributorsByDepartment() 
        {
            try
            {
                return await _chartService.GetContributorByDepart();
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Total Idea count for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("ideas-by-department")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<TotalIdeaOfDepartmentsResponseModel>>> GetTotalIdeaByDepartment()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.TotalIdeaOfEachDepartmentCacheKey, out List<TotalIdeaOfDepartmentsResponseModel> result)) { }
                else
                {
                    result = await _chartService.GetTotalIdeaOfEachDepartment();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.TotalIdeaOfEachDepartmentCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {

                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Get List of Ideas for dashboard.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("list-of-ideas")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<IdeaForChartResponseModel>>> GetIdeasForChart()
        {
            try
            {
                return await _chartService.GetIdeasForChart();
            }
            catch (Exception ex)
            {

                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Percentage Idea count for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("percentage-of-ideas-by-department")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<PercentageOfIdeaForEachDepartment>>> GetPercentageOfIdeasByDepartment()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.PercentageOfIdeasByDepartmentCacheKey, out List<PercentageOfIdeaForEachDepartment> result)) { }
                else
                {
                    result = await _chartService.GetPercentageOfIdeaForEachDepartments();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.PercentageOfIdeasByDepartmentCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {

                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Total staff and idea and comment and topic count.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("get-staff-idea-comment-topic")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<TotalStaffAndIdeaAndTopicAndCommentResponseModel>> GetTotalStaffAndIdeaAndCommentAndTopic()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey, out TotalStaffAndIdeaAndTopicAndCommentResponseModel result)) { }
                else
                {
                    result = await _chartService.GetTotalOfStaffAndIdeaAndTopicAndCommment();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {

                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Get the number of ideas with anonymous and idea with no comments for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("num-of-ideas-anonymous-by-department")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<NumOfIdeaAnonyByDepartment>>> GetNumOfIdeaAnonyAndNoCommentByDepart()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.NumOfIdeaAnonyAndNoCommentByDepartCacheKey, out List<NumOfIdeaAnonyByDepartment> result)) { }
                else
                {
                    result = await _chartService.GetNumOfIdeaAnonyAndNoCommentByDepart();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.NumOfIdeaAnonyAndNoCommentByDepartCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }
        
        /// <summary>
        /// Get the number of comments with anonymous and non-anonymous from users in each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("num-of-comment-by-department")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<NumOfCommentResponseModel>>> GetNumOfCommentByDepart()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.NumOfCommentByDepartCacheKey, out List<NumOfCommentResponseModel> result)) { }
                else
                {
                    result = await _chartService.GetNumOfCommentByDepart();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.NumOfCommentByDepartCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        /// <summary>
        /// Get the total of comments and ideas in 3 months by day.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("daily-report-in-three-months")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<ActionResult<List<DailyReportResponseModel>>> GetDailyReportInThreeMonths()
        {
            try
            {
                if (_cache.TryGetValue(_cacheKey.DailyReportInThreeMonthsCacheKey, out List<DailyReportResponseModel> result)) { }
                else
                {
                    result = await _chartService.GetDailyReportInThreeMonths();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(_cacheKey.DailyReportInThreeMonthsCacheKey, result, cacheEntryOptions);
                }
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }
    }
}
