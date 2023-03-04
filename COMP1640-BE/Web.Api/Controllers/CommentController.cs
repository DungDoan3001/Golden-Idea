using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Comment;
using Web.Api.DTOs;
using Web.Api.DTOs.RequestModels;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{ideaId}")]
        public async Task<ActionResult<List<CommentResponseModel>>> GetAllCommentOfIdea([FromRoute] Guid ideaId)
        {
            try
            {
                var comments = await _commentService.GetAllCommentOfIdea(ideaId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponseModel>> CreateComment([FromBody] CommentRequestModel comment)
        {
            try
            {
                var createComment = await _commentService.Create(comment);
                return Ok(createComment);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
