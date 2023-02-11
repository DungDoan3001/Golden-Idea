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

namespace Web.Api.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private RoleManager<IdentityRole<Guid>> roleManager;
        public RoleController(IRoleService roleService, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleService = roleService;
            this.roleManager = roleManager;
        }
        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <response code="200">Successfully get all the roles</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no roles</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<IdentityRole<Guid>>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAll();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        /// <summary>
        /// Create a role.
        /// </summary>
        /// <param name="roleName">Name of role</param>
        /// <returns>Add new role with roleName</returns>
        /// <response code="200">Successfully add the role with the given roleName</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while creating</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Create(string roleName)
        {
            try
            {
                var roles = await _roleService.Create(roleName);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        /// <summary>
        /// Update a role
        /// </summary>
        /// <param name="id">Id of the role will be updated.</param>
        /// <param name="roleUpdate">New name of the role for update</param>
        /// <returns>A role just updated</returns>
        /// <response code="200">Successfully updated the role</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while update a role</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Update([FromRoute] Guid id, string roleUpdate)
        {
            try
            {
                var roles = await _roleService.Update(id, roleUpdate);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        /// <summary>
        /// Delete a role by name
        /// </summary>
        /// <param name="roleName">Name of the role to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the role</response>
        /// <response code="204">Successfully deleted the role</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no role with the given roleName</response>
        [HttpDelete("{roleName}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Delete([FromRoute] string roleName)
        {
            try
            {
                var roles = await _roleService.Delete(roleName);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
