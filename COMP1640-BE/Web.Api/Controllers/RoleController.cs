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
        [HttpPut("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Update(Guid id, string roleUpdate)
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
        [HttpDelete("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Delete(string roleName)
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
