using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Extensions;
using Web.Api.Services.Category;
using System.Linq;
using Microsoft.Extensions.Logging;
using Web.Api.Services.EmailService;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Entities.Configuration;
using Web.Api.Configuration;

namespace Web.Api.Controllers
{
    [Route("api/categories")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;
        public CategoryController(IMapper mapper, ICategoryService categoryService, ILogger<CategoryController> logger, IMemoryCache cache, CacheKey cacheKey)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _logger = logger;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>List of Category objects</returns>
        /// <response code="200">Successfully get all categoriess</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetAll()
        {
            try
            {
                var getAllCacheKey = "GetAllCatagories";
                if (_cache.TryGetValue(getAllCacheKey, out IEnumerable<CategoryResponseModel> categoryResponses)) { }
                else
                {
                    _logger.LogInformation("Called");                  
                    IEnumerable<Entities.Category> categories = await _categoryService.GetAllAsync();
                    categoryResponses = _mapper.Map<IEnumerable<CategoryResponseModel>>(categories);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getAllCacheKey, categoryResponses.OrderBy(x => x.Name), cacheEntryOptions);
                    _cacheKey.CategoryCacheKey.Add(getAllCacheKey);
                }
                return Ok(categoryResponses.OrderBy(x => x.Name));
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
        /// Get a category by Id.
        /// </summary>
        /// <param name="id">Id of the category</param>
        /// <returns>A category with the given Id</returns>
        /// <response code="200">Successfully get the category with the given Id</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no category with the given Id</response>
        [HttpGet("{id}")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<CategoryResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                _logger.LogInformation("Called");
                var getByIdCacheKey = id.ToString() + "GetById";
                if (_cache.TryGetValue(getByIdCacheKey, out CategoryResponseModel departmentResponse)) { }
                else
                {
                    Category category = await _categoryService.GetByIdAsync(id);
                    if (category == null)
                    {
                        return NotFound(new MessageResponseModel
                        {
                            Message = "Not found.",
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Errors = new List<string> { "Can not find the category with the given id" }
                        });
                    }
                    departmentResponse = _mapper.Map<CategoryResponseModel>(category);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getByIdCacheKey, departmentResponse, cacheEntryOptions);
                    _cacheKey.IdeaCacheKey.Add(getByIdCacheKey);
                }
                return Ok(departmentResponse);
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
        /// Create a category
        /// </summary>
        /// <param name="requestModel">Request model for category</param>
        /// <returns>A category just created</returns>
        /// <response code="201">Successfully created the category</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a category</response>
        [HttpPost("")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<CategoryResponseModel>> Create([FromBody] CategoryRequestModel requestModel)
        {
            try
            {
                bool check = await CheckExist(requestModel.Name);
                if (check)
                    return Conflict(new MessageResponseModel 
                    { 
                        Message = "Conflict", 
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "The name already existed" }
                    });
                Entities.Category category = _mapper.Map<Entities.Category>(requestModel);
                Entities.Category createdCategory = await _categoryService.CreateAsync(category);
                if (createdCategory == null)
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Error while create new." }
                    });
                // Delete all category cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.CategoryCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return Created(createdCategory.Id.ToString(), _mapper.Map<CategoryResponseModel>(createdCategory));
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
        /// Update a category
        /// </summary>
        /// <param name="requestModel">Request model for category.</param>
        /// <param name="id">Id of the category to be updated.</param>
        /// <returns>A category just updated</returns>
        /// <response code="200">Successfully updated the category</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while update a category</response>
        [HttpPut("{id}")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<CategoryResponseModel>> Update([FromRoute] Guid id, [FromBody] CategoryRequestModel requestModel)
        {
            try
            {
                Entities.Category category = await _categoryService.GetByIdAsync(id);
                if (category == null) 
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors= new List<string> {"Can not find category with the given id"}
                    });
                if(category.Name.Trim().ToLower() != requestModel.Name.Trim().ToLower())
                {
                    bool check = await CheckExist(requestModel.Name);
                    if (check)
                        return Conflict(new MessageResponseModel
                        {
                            Message = "Conflict",
                            StatusCode = (int)HttpStatusCode.Conflict,
                            Errors = new List<string> { "The name already existed" }
                        });
                }
                _mapper.Map<CategoryRequestModel, Entities.Category>(requestModel, category);
                Entities.Category updatedCategory = await _categoryService.UpdateAsync(category);
                // Delete all category cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.CategoryCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return Ok(_mapper.Map<CategoryResponseModel>(updatedCategory));
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
        /// Delete a category
        /// </summary>
        /// <param name="id">Id of the category to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully delete the category</response>
        /// <response code="204">Successdelete the category</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no category with the given Id</response>
        [HttpDelete("{id}")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager)] // Roles Here
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Entities.Category category = await _categoryService.GetByIdAsync(id);

                if (category == null) 
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find category with the given id" }
                    });

                if(category.Ideas.Count() > 0)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Sorry! We cannot delete the category because it contains some ideas." }
                    });
                }

                bool isDelete = await _categoryService.DeleteAsync(id);
                if (!isDelete)
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not Found", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Error while update." }
                    });
                // Delete all category cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.CategoryCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return NoContent();
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

        private async Task<bool> CheckExist(string name)
        {
            IEnumerable<Entities.Category> checkDepartments = await _categoryService.GetByNameAsync(name);
            if (checkDepartments.Any())
                return true;
            return false;
        }
    }
}
