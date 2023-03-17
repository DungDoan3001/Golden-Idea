using System;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;

namespace Web.Api.Services.ReactionService
{
    public interface IReactionService
    {
        Task<GetUserReactionResponseModel> GetReactionOfUserInIdea(string username, Guid ideaId);
        Task<Reaction> Reaction(string username, Guid ideaId, string reactionType);
    }
}