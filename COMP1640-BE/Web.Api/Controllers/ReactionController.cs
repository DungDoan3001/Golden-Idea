using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Extensions;
using Web.Api.Services.Category;
using Web.Api.Services.ReactionService;
using System;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReactionService _reactionService;
        public ReactionController(IReactionService reactionService, IMapper mapper)
        {
            _reactionService = reactionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get reaction of user in the idea.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet]
        public async Task<ActionResult<GetUserReactionResponseModel>> GetUserReactionInIdea([FromQuery] GetUserReactionRequestModel userReaction)
        {
            try
            {
                var result = await _reactionService.GetReactionOfUserInIdea(userReaction.Email, userReaction.IdeaId);
                if(result == null) 
                { 
                    return NotFound(new MessageResponseModel
                    {
                        Message = "Error",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find the reaction due to wrong user or idea!" }
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
        /// <summary>
        /// Create a reaction
        /// </summary>
        /// <param name="reactionType">Reaction type (upvote, downvote)</param>
        /// <param name="userReact">Reaction request model</param>
        /// <returns>A reaction just created</returns>
        /// <response code="201">Successfully created the reaction</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is no reaction type provided.</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ReactionResponseModel>> Create([FromQuery] string reactionType, [FromBody] ReactionRequestModel userReact)
        {
            try
            {
                if (reactionType == null) 
                { 
                    return Conflict(new MessageResponseModel 
                    { 
                        Message = "Conflict", 
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors= new List<string> { "No reaction type provided!" }
                    });
                }
                var reaction = await _reactionService.Reaction(userReact.userId, userReact.ideaId, reactionType);
                var result = _mapper.Map<ReactionResponseModel>(reaction);
                if (result == null)
                {
                    return Ok(new MessageResponseModel
                    {
                        Message = "Successfull delete a reaction",
                        StatusCode = (int)HttpStatusCode.OK
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
