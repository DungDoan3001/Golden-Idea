using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.IdeaService;
using Web.Api.Entities;
using AutoMapper;
using Web.Api.DTOs.RequestModels;
using Web.Api.Extensions;
using System.Linq;

namespace Web.Api.Controllers
{
    [Route("api/ideas")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIdeaService _ideaService;

        public IdeaController(IMapper mapper, IIdeaService ideaService)
        {
            _mapper = mapper;
            _ideaService = ideaService;
        }

        /// <summary>
        /// Get all topics.
        /// </summary>
        /// <returns>List of Topic objects</returns>
        /// <response code="200">Successfully get all topics</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<IdeaResponeModel>>> GetAll()
        {
            try
            {
                IEnumerable<Idea> topics = await _ideaService.GetAllAsync();
                IEnumerable<IdeaResponeModel> TopicResponses = _mapper.Map<IEnumerable<IdeaResponeModel>>(topics);
                return Ok(TopicResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        /// <summary>
        /// Create a topic
        /// </summary>
        /// <param name="requestModel">Request model for topic</param>
        /// <returns>A topic just created</returns>
        /// <response code="201">Successfully created the topic</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a topic</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromForm] IdeaRequestModel requestModel)
        {
            try
            {
                //bool check = await CheckExist(requestModel.Name);
                //if (check)
                //    return Conflict(new MessageResponseModel { Message = "The name already existed", StatusCode = (int)HttpStatusCode.Conflict });
                Idea idea = _mapper.Map<Idea>(requestModel);
                Idea createdIdea = await _ideaService.CreateAsync(idea);
                if (createdIdea == null)
                    return Conflict(new MessageResponseModel { Message = "Error while create new.", StatusCode = (int)HttpStatusCode.Conflict });
                return Created(createdIdea.Id.ToString(), _mapper.Map<TopicResponseModel>(createdIdea));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
