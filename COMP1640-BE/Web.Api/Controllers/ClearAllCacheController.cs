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
using Web.Api.Configuration;

namespace Web.Api.Controllers
{
    [Route("api/clear-all-cache")]
    [Authorize]
    [ApiController]
    public class ClearAllCacheController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        private CacheKey _cacheKey;
        public ClearAllCacheController(IMemoryCache cache, CacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        [HttpPost("")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<IActionResult> ClearAllCache()
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.GetType().GetProperties())
                    {
                        var type = key.PropertyType;
                        if (type == typeof(List<string>))
                        {
                            List<string> value = (List<string>)key.GetValue(_cacheKey);
                            foreach (var item in value)
                            {
                                _cache.Remove(item);
                            }
                        }
                    }
                });
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
