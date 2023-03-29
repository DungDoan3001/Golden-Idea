using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.User;
using Web.Api.DTOs.RequestModels;
using Web.Api.Extensions;
using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using static Web.Api.Configuration.CacheKey;
using Web.Api.Entities.Configuration;
using Web.Api.Configuration;
using Web.Api.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<Entities.User> _userManager;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;
        public UserController(IUserService userService, IMapper mapper, UserManager<Entities.User> userManager, IMemoryCache cache, CacheKey cacheKey)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _cache = cache;
            _cacheKey = cacheKey;
        }
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <response code="200">Successfully get all the users</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<ActionResult<List<UserResponseModel>>> GetAll()
        {
            try
            {
                var getAllCacheKey = "GetAllUsers";
                if (_cache.TryGetValue(getAllCacheKey, out List<UserResponseModel> result)) { }
                else
                {
                    var users = await _userService.GetAll();
                    result = _mapper.Map<List<UserResponseModel>>(users);
                    //Get role for all user
                    for (int i = 0; i < users.Count; i++)
                    {
                        var role = await _userManager.GetRolesAsync(users[i]);
                        foreach (var r in role)
                        {
                            result[i].Role = r;
                        }
                    }
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getAllCacheKey, result.OrderBy(x => x.Name), cacheEntryOptions);
                    _cacheKey.UserCacheKey.Add(getAllCacheKey);
                }
                              
                return Ok(result.OrderBy(x => x.Name));
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
        /// Get all users with Staff role.
        /// </summary>
        /// <response code="200">Successfully get all the users with Staff role</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("getallstaff")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<ActionResult<List<UserResponseModel>>> GetAllStaff()
        {
            try
            {
                var getAllCacheKey = "GetAllStaff";
                if (_cache.TryGetValue(getAllCacheKey, out List<UserResponseModel> result)) { }
                else
                {
                    var users = await _userService.GetAllStaff();
                    result = _mapper.Map<List<UserResponseModel>>(users);
                    //Get role for all user
                    for (int i = 0; i < users.Count; i++)
                    {
                        var role = await _userManager.GetRolesAsync(users[i]);
                        foreach (var r in role)
                        {
                            result[i].Role = r;
                        }
                    }
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getAllCacheKey, result.OrderBy(x => x.Name), cacheEntryOptions);
                    _cacheKey.UserCacheKey.Add(getAllCacheKey);
                }
               
                return Ok(result.OrderBy(x => x.Name));
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
        /// Get all users with Admin and QA role.
        /// </summary>
        /// <response code="200">Successfully get all the users with Admin and QA role</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("getalladminQA")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<ActionResult<List<UserResponseModel>>> GetAllAdminQA()
        {
            try
            {
                var getAllCacheKey = "GetAllAdminQA";
                if (_cache.TryGetValue(getAllCacheKey, out List<UserResponseModel> result)) { }
                else
                {
                    var users = await _userService.GetAllAdminQA();
                    result = _mapper.Map<List<UserResponseModel>>(users);
                    //Get role for all user
                    for (int i = 0; i < users.Count; i++)
                    {
                        var role = await _userManager.GetRolesAsync(users[i]);
                        foreach (var r in role)
                        {
                            result[i].Role = r;
                        }
                    }
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getAllCacheKey, result.OrderBy(x => x.Name), cacheEntryOptions);
                    _cacheKey.UserCacheKey.Add(getAllCacheKey);
                }
                
                return Ok(result.OrderBy(x => x.Name));
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
        /// Get user information by id.
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <response code="200">Successfully get the user information</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no user with the given Id</response>
        [HttpGet("{id}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<ActionResult<UserResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                var getByIdCacheKey = id.ToString() + "GetByIdCacheKey";
                if (_cache.TryGetValue(getByIdCacheKey, out UserResponseModel result)) { }
                else
                {
                    var user = await _userService.GetById(id);
                    if (user == null)
                    {
                        return NotFound(new MessageResponseModel
                        {
                            Message = "Not Found",
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Errors = new List<string> { "Can not find the user" }
                        });
                    }
                    result = _mapper.Map<UserResponseModel>(user);
                    //Get role
                    var role = await _userManager.GetRolesAsync(user);
                    foreach (var r in role)
                    {
                        result.Role = r;
                    }
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getByIdCacheKey, result, cacheEntryOptions);
                    _cacheKey.IdeaCacheKey.Add(getByIdCacheKey);
                }   
                return Ok(result);
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
        /// Update a user 
        /// </summary>
        /// <param name="id">Id of the user will be updated.</param>
        /// <param name="user">User request model</param>
        /// <returns>A user updated</returns>
        /// <response code="200">Successfully updated the user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while update a user</response>
        [HttpPut("{id}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromForm] UserForUpdateRequestModel user)
        {
            try
            { 
                var updateUser = await _userService.UpdateAsync(id, user);
                if (updateUser == null)
                {
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not Found", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Update error! Can not find the user to update!" }
                    });
                }
                var result = _mapper.Map<UserResponseModel>(updateUser);
                result.Role = user.Role;
                // Delete all user cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.IdeaCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return Ok(result);
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
        /// Delete a user
        /// </summary>
        /// <param name="id">Id of the user to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the user</response>
        /// <response code="204">Successfully deleted the user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no user with the given Id</response>
        [HttpDelete("{id}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _userService.Delete(id);
                // Delete all user cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.IdeaCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return Ok(result);
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
