using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Services.DepartmentService;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IDepartmentService _departmentService;
        public AuthenticationController(IMapper mapper, UserManager<User> userManager, IDepartmentService departmentService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _departmentService = departmentService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationRequestModel userForRegistration)
        {
            try
            {
                var user = _mapper.Map<User>(userForRegistration);
                var result = await _userManager.CreateAsync(user, userForRegistration.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
                //var department = await _departmentService.GetAllAsync();
                //foreach(var d in department)
                //{
                //    if(userForRegistration.Departments )
                //}
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
            
        }
    }
}
