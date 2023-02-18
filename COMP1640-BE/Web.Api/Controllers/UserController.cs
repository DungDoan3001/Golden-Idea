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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <response code="200">Successfully get all the users</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet]
        public async Task<ActionResult<List<UserResponseModel>>> GetAll() //can not get role
        {
            try
            {
                var roles = await _userService.GetAll();
                //var result = _mapper.Map<List<UserResponseModel>>(roles);
                return Ok(roles);
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
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UserRequestModel user)
        {
            try
            {
                var userUpdate = _mapper.Map<Entities.User>(user);
                var updateUser = await _userService.UpdateAsync(id, userUpdate); //error
                var result = _mapper.Map<UserResponseModel>(updateUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

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
