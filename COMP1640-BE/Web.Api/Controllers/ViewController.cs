using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Extensions;
using Web.Api.Services.View;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {
        private readonly IViewService _viewService;
        private readonly IMapper _mapper;
        public ViewController(IViewService viewService, IMapper mapper)
        {
            _viewService = viewService;
            _mapper = mapper;
        }

        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ViewResponseModel>> Create([FromQuery] Guid ideaId, [FromBody] ViewRequestModel userView)
        {
            try
            {
                var view = await _viewService.ViewCount(ideaId, userView.Username);
                var result = _mapper.Map<ViewResponseModel>(view);
                if (result == null)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Can not create view!" }
                    });
                }
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

    }
}
