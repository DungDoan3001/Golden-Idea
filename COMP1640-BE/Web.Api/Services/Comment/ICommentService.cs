using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Api.DTOs;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.Comment
{
    public interface ICommentService
    {
        Task<CommentResponseModel> Create(CommentRequestModel comment);
        Task<List<CommentResponseModel>> GetAllCommentOfIdea(Guid ideaId);
        Task<bool> DeleteByIdeaAsync(Guid ideaId);
    }
}