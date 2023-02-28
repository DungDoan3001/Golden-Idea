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
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Services.ImageService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    //[Authorize(Roles ="Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<Entities.User> _userManager;
        public UserController(IUserService userService, IMapper mapper, UserManager<Entities.User> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <response code="200">Successfully get all the users</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet]
        public async Task<ActionResult<List<UserResponseModel>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                var result = _mapper.Map<List<UserResponseModel>>(users);
                //Get role for all user
                for(int i = 0; i < users.Count; i++)
                {
                    var role = await _userManager.GetRolesAsync(users[i]);
                    foreach(var r in role)
                    {
                        result[i].Role = r;
                    }
                }                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
        public async Task<ActionResult<UserResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                var user = await _userService.GetById(id);
                if(user == null)
                {
                    return NotFound(new MessageResponseModel { Message = "Can not find the user", StatusCode = (int)HttpStatusCode.BadRequest });
                }
                var result = _mapper.Map<UserResponseModel>(user);
                //Get role
                var role = _userManager.GetRolesAsync(user);
                foreach(var r in role.Result)
                {
                    result.Role = r;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id">Id of the user will be updated.</param>
        /// <param name="userUpdate">New name of the user for update</param>
        /// <returns>A user updated</returns>
        /// <response code="200">Successfully updated the user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while update a user</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromForm] UserRequestModel user)
        {
            try
            { 
                var updateUser = await _userService.UpdateAsync(id, user);
                if (updateUser == null)
                {
                    return NotFound(new MessageResponseModel { Message = "Update error! Can not find the user to update!", StatusCode = (int)HttpStatusCode.NotFound });
                }
                var result = _mapper.Map<UserResponseModel>(updateUser);
                result.Role = user.Role;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _userService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
