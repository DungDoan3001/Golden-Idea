using System;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Services.ReactionService
{
    public interface IReactionService
    {
        Task<Reaction> Reaction(Guid userId, Guid ideaId, string reactionType);
    }
}