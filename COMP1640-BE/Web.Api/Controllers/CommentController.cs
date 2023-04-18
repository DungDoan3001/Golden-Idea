using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Comment;
using Web.Api.DTOs.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Entities.Configuration;
using Web.Api.Extensions;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Get all comments of specific idea.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("{ideaId}")]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<List<CommentResponseModel>>> GetAllCommentOfIdea([FromRoute] Guid ideaId)
        {
            try
            {
                var comments = await _commentService.GetAllCommentOfIdea(ideaId);
                return Ok(comments);
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
        /// Create a comment in the idea
        /// </summary>
        /// <param name="comment">Request model for comment</param>
        /// <returns>A category just created</returns>
        /// <response code="201">Successfully created a comment for the idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a comment</response>
        [HttpPost]
        [Roles(IdentityRoles.Administrator, IdentityRoles.QAManager, IdentityRoles.QACoordinator, IdentityRoles.Staff)] // Roles Here
        public async Task<ActionResult<CommentResponseModel>> CreateComment([FromBody] CommentRequestModel comment)
        {
            try
            {
                var createComment = await _commentService.Create(comment);
                return Ok(createComment);
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
