using System;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;

namespace Web.Api.Services.ReactionService
{
    public interface IReactionService
    {
        Task<GetUserReactionResponseModel> GetReactionOfUserInIdea(string email, Guid ideaId);
        Task<Reaction> Reaction(Guid userId, Guid ideaId, string reactionType);
    }
}