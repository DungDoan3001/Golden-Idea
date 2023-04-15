using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Configuration;
using Web.Api.Data.Context;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Web.Api.Entities.Configuration;
using Web.Api.Extensions;
using Web.Api.Services.Authentication;
using Web.Api.Services.FileUploadService;
using Web.Api.Services.ResetPassword;

namespace Web.Api.Controllers
{
    [Route("api/authentication")]
    [Authorize]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Entities.Role> roleManager;
        private readonly IAuthenticationManager _authManager;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMemoryCache _cache;
        protected AppDbContext _context;
        private CacheKey _cacheKey;
        public AuthenticationController(IMapper mapper, UserManager<User> userManager, RoleManager<Entities.Role> roleManager, IAuthenticationManager authManager, IResetPasswordService resetPasswordService, IFileUploadService fileUploadService, IMemoryCache cache, CacheKey cacheKey, AppDbContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            this.roleManager = roleManager;
            _authManager = authManager;
            _resetPasswordService = resetPasswordService;
            _fileUploadService = fileUploadService;
            _cache = cache;
            _cacheKey = cacheKey;
            _context = context;
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
        [Roles(IdentityRoles.Administrator)] // Roles Here
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromForm] UserRequestModel userForRegistration)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(userForRegistration.Role))
                {
                    return NotFound(new MessageResponseModel { Message = "The role does not existed!", StatusCode = (int)HttpStatusCode.NotFound });
                }
                var usersInRole = await _userManager.GetUsersInRoleAsync(userForRegistration.Role);
                foreach(var userInRole in usersInRole)
                {
                    if(userInRole.DepartmentId == userForRegistration.DepartmentId)
                    {
                        return Conflict(new MessageResponseModel { Message = "The department has exist a QA Manager, each department has only one QA Manager", StatusCode = (int)HttpStatusCode.NotFound });
                    }
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
                if (userForRegistration.File != null)
                {
                    var imageResult = await _fileUploadService.UploadImageAsync(userForRegistration.File);

                    if (imageResult.Error != null)
                        return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                    user.Avatar = imageResult.SecureUrl.ToString();
                    user.PublicId = imageResult.PublicId;
                }

                var create = await _userManager.CreateAsync(user, userForRegistration.Password);
                if (!create.Succeeded)
                {
                    foreach (var error in create.Errors)
                    {
                        return NotFound(new MessageResponseModel { Message = error.Description, StatusCode = (int)HttpStatusCode.Conflict });
                    }
                }
                await _userManager.AddToRoleAsync(user, userForRegistration.Role);
                //Send email to user
                await _authManager.SendEmailRegister(userForRegistration);
                //Get user data (id + role) to response
                var data = await _userManager.FindByNameAsync(user.UserName);
                var result = _mapper.Map<UserResponseModel>(data);
                result.Role = userForRegistration.Role;
                // Delete all user cache
                await Task.Run(() =>
                {
                    foreach (var key in _cacheKey.UserCacheKey)
                    {
                        _cache.Remove(key);
                    }
                    // Delete cache for chart TotalStaffAndIdeaAndCommentAndTopic
                    _cache.Remove(_cacheKey.TotalStaffAndIdeaAndCommentAndTopicCacheKey);
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
        /// Login or Authenticate a user.
        /// </summary>
        /// <param name="user">Request model for authentication</param>
        /// <returns>Token of user</returns>
        /// <response code="200">Successfully create a token for user</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while authenticating</response>
        [HttpPost("login")]
        [AllowAnonymous]
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
        /// Send email to reset password.
        /// </summary>
        /// <param name="user">Request model for send email change password</param>
        /// <returns>Message to email including changepass token and id</returns>
        /// <response code="200">Successfully sending email</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while sending email</response>
        [HttpPost("change-password")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SendEmailChangePassword([FromBody] SendEmailChangePassword user)
        {
            var getUser = await _userManager.FindByEmailAsync(user.Email.ToUpper());
            if (getUser != null)
            {
                bool changePass = await _authManager.GenerateChangePasswordTokenAsync(getUser);
                if (changePass)
                {
                    return Ok(new MessageResponseModel {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "The email reset password has sent!" });
                }
            }
            return BadRequest(new MessageResponseModel
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Please make sure that the Email is correct!"
            });
        }
        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="resetPasswordIdEncoded">Id of reset password</param>
        /// <param name="password">Request model for change password</param>
        /// <returns>Change password for user</returns>
        /// <response code="200">Successfully changing password</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is a conflict while changing password</response>
        [HttpPut("change-password/{resetPasswordIdEncoded}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangePassword([FromRoute] string resetPasswordIdEncoded, [FromBody] ChangePasswordRequestModel password)
        {
            Guid resetPasswordIdDecoded = Helpers.GuidBase64.Decode(resetPasswordIdEncoded);
            var resetPassword = await _resetPasswordService.GetByIdAsync(resetPasswordIdDecoded);
            if(resetPassword == null)
            {
                return BadRequest(new MessageResponseModel
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Wrong identity, please make sure your link is correct!"
                });
            }
            var checkExpire = DateTime.Compare(resetPassword.CreatedDate.AddHours(24), DateTime.Now);
            if (checkExpire == -1)
            {
                await _resetPasswordService.DeleteAsync(resetPasswordIdDecoded);
                return BadRequest(new MessageResponseModel
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Sorry, your reset password link is expired! (more than 24h)"
                });
            }
            var user = await _userManager.FindByIdAsync(resetPassword.UserId.ToString());
            if(password.Password != password.ConfirmPassword)
            {
                return BadRequest(new MessageResponseModel { 
                    StatusCode = (int)HttpStatusCode.BadRequest, 
                    Message = "Please make sure that your password and confirm password are the same!"});
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, password.Password);
            if(!result.Succeeded)
            {
                foreach(var e in result.Errors)
                {
                    return BadRequest(new MessageResponseModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = e.Description });
                }
            }
            return Ok(result);
        }
    }
}

