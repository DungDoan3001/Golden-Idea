using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Role;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using Web.Api.Extensions;
using Web.Api.DTOs.RequestModels;
using AutoMapper;
using System.Linq;

namespace Web.Api.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <response code="200">Successfully get all the roles</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no roles</response>
        [HttpGet]
        public async Task<ActionResult<List<IdentityRole<Guid>>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAll();
                return Ok(roles);
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
        /// Create a role.
        /// </summary>
        /// <param name="requestModel">Role request model</param>
        /// <returns>Add new role with roleName</returns>
        /// <response code="200">Successfully add the role with the given roleName</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while creating</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Create([FromBody] RoleRequestModel requestModel)
        {
            try
            {
                var roles = await _roleService.Create(requestModel.Name);
                return Ok(roles);
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
        /// Update a role
        /// </summary>
        /// <param name="id">Id of the role will be updated.</param>
        /// <param name="requestModel">Role request model</param>
        /// <returns>A role just updated</returns>
        /// <response code="200">Successfully updated the role</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while update a role</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] RoleRequestModel requestModel)
        {
            try
            {
                var roles = await _roleService.Update(id, requestModel.Name);
                return Ok(roles);
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
        /// Delete a role by name
        /// </summary>
        /// <param name="requestModel">Role request model.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the role</response>
        /// <response code="204">Successfully deleted the role</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no role with the given roleName</response>
        [HttpDelete("{roleName}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Delete([FromBody] RoleRequestModel requestModel)
        {
            try
            {
                var roles = await _roleService.Delete(requestModel.Name);
                return Ok(roles);
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
