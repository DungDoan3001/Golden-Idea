using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.FakeData;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Entities.Configuration;
using Web.Api.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Configuration;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FakeDataGenController : ControllerBase
    {
        private readonly IFakeDataService _fakeDataService;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;
        public FakeDataGenController(IFakeDataService fakeDataService, IMemoryCache cache, CacheKey cacheKey)
        {
            _fakeDataService = fakeDataService;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        /// <summary>
        /// Create fake data base on the number of data want to fake (number from 1 to 200)
        /// </summary>
        [HttpPost("{numberToGenerate}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetAll([FromRoute] int numberToGenerate)
        {
            try
            {
                if(numberToGenerate < 0 && numberToGenerate > 200)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Passed the limit of numberToGenerate" }
                    });
                }
                var result = await _fakeDataService.CreateFakeIdeaData(numberToGenerate);
                if (result)
                {
                    // Delete all idea cache
                    await Task.Run(() =>
                    {
                        foreach (var key in _cacheKey.IdeaCacheKey)
                        {
                            _cache.Remove(key);
                        }
                        // Delete cache for Exception Report Chart Idea
                        _cache.Remove(_cacheKey.NumOfIdeaAnonyAndNoCommentByDepartCacheKey);
                        // Delete cache for GetPercentageOfIdeaForEachDepartments chart
                        _cache.Remove(_cacheKey.PercentageOfIdeasByDepartmentCacheKey);
                        // Delete cache for chart TotalStaffAndIdeaAndCommentAndTopic
                        _cache.Remove(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey);
                        // Delete cache for TotalIdeaOfEachDepartmentCacheKey chart
                        _cache.Remove(_cacheKey.TotalIdeaOfEachDepartmentCacheKey);
                        // Delete cache for GetDailyReportInThreeMonths chart
                        _cache.Remove(_cacheKey.DailyReportInThreeMonthsCacheKey);
                    });
                    return Ok(new MessageResponseModel
                    {
                        Message = "Created successfully " + numberToGenerate + " ideas.",
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                return Conflict(new MessageResponseModel
                {
                    Message = "Conflict",
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Errors = new List<string> {"The result return false." }
                });
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

        [HttpDelete("")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var result = await _fakeDataService.DeleteFakeDataAsync();
                if (result)
                {
                    // Delete all idea cache
                    await Task.Run(() =>
                    {
                        foreach (var key in _cacheKey.IdeaCacheKey)
                        {
                            _cache.Remove(key);
                        }
                        // Delete cache for Exception Report Chart Idea
                        _cache.Remove(_cacheKey.NumOfIdeaAnonyAndNoCommentByDepartCacheKey);
                        // Delete cache for GetPercentageOfIdeaForEachDepartments chart
                        _cache.Remove(_cacheKey.PercentageOfIdeasByDepartmentCacheKey);
                        // Delete cache for chart TotalStaffAndIdeaAndCommentAndTopic
                        _cache.Remove(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey);
                        // Delete cache for TotalIdeaOfEachDepartmentCacheKey chart
                        _cache.Remove(_cacheKey.TotalIdeaOfEachDepartmentCacheKey);
                        // Delete cache for GetDailyReportInThreeMonths chart
                        _cache.Remove(_cacheKey.DailyReportInThreeMonthsCacheKey);
                    });

                    return Ok(new MessageResponseModel
                    {
                        Message = "Deleted successfully every fake idea data",
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                return Conflict(new MessageResponseModel
                {
                    Message = "Conflict",
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Errors = new List<string> { "The result return false." }
                });
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
