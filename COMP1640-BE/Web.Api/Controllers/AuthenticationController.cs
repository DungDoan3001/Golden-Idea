using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Xml.Linq;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Services.Authentication;
using Web.Api.Services.DepartmentService;

namespace Web.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Entities.Role> roleManager;
        private readonly IAuthenticationManager _authManager;

        public AuthenticationController(IMapper mapper, UserManager<User> userManager, RoleManager<Entities.Role> roleManager, IAuthenticationManager authManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            this.roleManager = roleManager;
            _authManager = authManager;
        }
        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="userForRegistration">Request model for register</param>
        /// <returns>Add a new user</returns>
        /// <response code="200">Successfully register a User</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while creating</response>
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationRequestModel userForRegistration)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(userForRegistration.Role))
                {
                    return NotFound(new MessageResponseModel { Message = "The role does not existed!", StatusCode = (int)HttpStatusCode.NotFound });
                }
                if (await _userManager.FindByEmailAsync(userForRegistration.Email) != null)
                {
                    return NotFound(new MessageResponseModel { Message = "The email has existed!", StatusCode = (int)HttpStatusCode.NotFound });
                }
                else if (!new EmailAddressAttribute().IsValid(userForRegistration.Email))
                {
                    return NotFound(new MessageResponseModel { Message = "The email is not valid!", StatusCode = (int)HttpStatusCode.NotFound });
                }
                var user = _mapper.Map<User>(userForRegistration);
                var create = await _userManager.CreateAsync(user, userForRegistration.Password);
                if (!create.Succeeded)
                {
                    foreach (var error in create.Errors)
                    {
                        return NotFound(new MessageResponseModel { Message = error.Description, StatusCode = (int)HttpStatusCode.Conflict });
                    }
                }
                await _userManager.AddToRoleAsync(user, userForRegistration.Role);
                //Get user data (id + role) to response
                var data = await _userManager.FindByNameAsync(user.UserName);
                var result = _mapper.Map<UserForRegistrationResponseModel>(data);
                result.Role = userForRegistration.Role;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        /// <summary>
        /// Login or Authenticate a user.
        /// </summary>
        /// <param name="user">Request model for authentication</param>
        /// <returns>Token of user</returns>
        /// <response code="200">Successfully create a token for user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while authenticating</response>
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenRequestModel user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                return NotFound(new MessageResponseModel { Message = "Email or password is incorrect, please try again!", StatusCode = (int)HttpStatusCode.NotFound });
            }
            UserForAuthenResponseModel result = new UserForAuthenResponseModel()
            {
                Token = await _authManager.CreateToken(),
                Status = "Success",
                Message = "Authentication is success!"
            };
            return Ok(result);
        }

        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="request">Request model for logout</param>
        /// <returns>Remove authentication of user</returns>
        /// <response code="200">Successfully remove authentication for user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while remove</response>
        //[Authorize]
        //[HttpPost("logout")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> LogOut() //in process NOT DONE
        //{
        //    try
        //    {
        //        Response.Headers.Remove("Authorization");
        //        //HttpContext.Session.Clear();
        //        return Ok(new MessageResponseModel
        //        {
        //            Message = "Logout successfull!",
        //            StatusCode = (int)HttpStatusCode.OK
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
        //    }
        //}
    }
}

