using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Configuration;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Web.Api.Entities.Configuration;
using Web.Api.Extensions;
using Web.Api.Services.DepartmentService;
using static Web.Api.Configuration.CacheKey;

namespace Web.Api.Controllers
{
    [Route("api/departments")]
    [Authorize]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly IMemoryCache _cache;
        private CacheKey _cacheKey;
        public DepartmentController(IMapper mapper, IDepartmentService departmentService, IMemoryCache cache, CacheKey cacheKey)
        {
            _mapper = mapper;
            _departmentService = departmentService;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        /// <summary>
        /// Get all departments.
        /// </summary>
        /// <returns>List of Department objects</returns>
        /// <response code="200">Successfully get all departments</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<IEnumerable<DepartmentResponseModel>>> GetAll()
        {
            try
            {
                var getAllCacheKey = "GetAllDepartments";
                if (_cache.TryGetValue(getAllCacheKey, out IEnumerable<DepartmentResponseModel> departmentResponse)) { }
                else
                {
                    IEnumerable<Department> departments = await _departmentService.GetAllAsync();
                    departmentResponse = _mapper.Map<IEnumerable<DepartmentResponseModel>>(departments);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(getAllCacheKey, departmentResponse.OrderBy(x => x.Name), cacheEntryOptions);
                    _cacheKey.DepartmentCacheKey.Add(getAllCacheKey);
                }
                return Ok(departmentResponse.OrderBy(x => x.Name));
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
        /// Get a department by Id.
        /// </summary>
        /// <param name="id">Id of the department</param>
        /// <returns>A department with the given Id</returns>
        /// <response code="200">Successfully get the department with the given Id</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no department with the given Id</response>
        [HttpGet("{id}")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<DepartmentResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                var getByIdCacheKey = id.ToString() + "GetById";
                if (_cache.TryGetValue(getByIdCacheKey, out DepartmentResponseModel departmentResponse)) { }
                else
                {
                    Department department = await _departmentService.GetByIdAsync(id);
                    if (department == null)
                    {
                        return NotFound(new MessageResponseModel
                        {
                            Message = "Not found.",
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Errors = new List<string> { "Can not find department with the given id" }
                        });
                    }
                    departmentResponse = _mapper.Map<DepartmentResponseModel>(department);
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
        /// Create a department
        /// </summary>
        /// <param name="requestModel">Request model for department</param>
        /// <returns>A department just created</returns>
        /// <response code="201">Successfully created the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a department</response>
        [HttpPost("")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<DepartmentResponseModel>> Create([FromBody] DepartmentRequestModel requestModel)
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
                Department department = _mapper.Map<Department>(requestModel);
                Department createdDepartment = await _departmentService.CreateAsync(department);
                if (createdDepartment == null)
                    return Conflict(new MessageResponseModel 
                    { 
                        Message = "Conflict", 
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Error while create new." }
                    });
                // Delete all department cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.DepartmentCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                return Created(createdDepartment.Id.ToString(), _mapper.Map<DepartmentResponseModel>(createdDepartment));
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
        /// Update a department
        /// </summary>
        /// <param name="requestModel">Request model for department.</param>
        /// <param name="id">Id of the department to be updated.</param>
        /// <returns>A department just updated</returns>
        /// <response code="200">Successfully updated the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while update a department</response>
        [HttpPut("{id}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<DepartmentResponseModel>> Update([FromRoute] Guid id, [FromBody] DepartmentRequestModel requestModel)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null) 
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find department with the given id" }
                    });
                if(department.Name.Trim().ToLower() != requestModel.Name.Trim().ToLower())
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
                // Delete all department cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.DepartmentCacheKey)
                    {
                        _cache.Remove(key);
                    }
                });
                _mapper.Map<DepartmentRequestModel, Department>(requestModel, department);
                Department updatedDepartment = await _departmentService.UpdateAsync(department);
                return Ok(_mapper.Map<DepartmentResponseModel>(updatedDepartment));
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
        /// Delete a department
        /// </summary>
        /// <param name="id">Id of the department to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the department</response>
        /// <response code="204">Successfully deleted the department</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no department with the given Id</response>
        [HttpDelete("{id}")]
        [Roles(IdentityRoles.Administrator)] // Roles Here
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null) 
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find department with the given id" }
                    });

                if(department.Users.Count() > 0)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode= (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Sorry! We cannot delete the department because it contains some users." }
                    });
                }

                bool isDelete = await _departmentService.DeleteAsync(id);
                if (!isDelete)
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not Found", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Error while delete." }
                    });
                // Delete all department cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.DepartmentCacheKey)
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
            IEnumerable<Department> checkDepartments = await _departmentService.GetByNameAsync(name);
            if (checkDepartments.Any())
                return true;
            return false;
        }
    }
}
