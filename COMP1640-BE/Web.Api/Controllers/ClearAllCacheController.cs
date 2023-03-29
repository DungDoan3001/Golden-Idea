using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static Web.Api.Configuration.CacheKey;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Extensions;
using Web.Api.Entities.Configuration;

namespace Web.Api.Controllers
{
    [Route("api/clear-all-cache")]
    [Authorize]
    [ApiController]
    public class ClearAllCacheController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        public ClearAllCacheController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<IActionResult> ClearAllCache()
        {
            try
            {
                //await Task.Run(() =>
                //{
                //    TopicCacheKey topicCacheKey = new TopicCacheKey();
                //    UserCacheKey userCacheKey = new UserCacheKey();
                //    CategoryCacheKey categoryCacheKey = new CategoryCacheKey();
                //    DepartmentCacheKey departmentCacheKey = new DepartmentCacheKey();
                //    IdeaCacheKey ideaCacheKey = new IdeaCacheKey();
                //    // Delete all idea cache
                //    foreach (var key in topicCacheKey.GetType().GetProperties())
                //    {
                //        _cache.Remove(key.GetValue(topicCacheKey));
                //    }

                //    foreach (var key in userCacheKey.GetType().GetProperties())
                //    {
                //        _cache.Remove(key.GetValue(userCacheKey));
                //    }
                //    foreach (var key in categoryCacheKey.GetType().GetProperties())
                //    {
                //        _cache.Remove(key.GetValue(categoryCacheKey));
                //    }
                //    foreach (var key in departmentCacheKey.GetType().GetProperties())
                //    {
                //        _cache.Remove(key.GetValue(departmentCacheKey));
                //    }
                //    foreach (var key in ideaCacheKey.GetType().GetProperties())
                //    {
                //        _cache.Remove(key.GetValue(ideaCacheKey));
                //    }
                //});
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
