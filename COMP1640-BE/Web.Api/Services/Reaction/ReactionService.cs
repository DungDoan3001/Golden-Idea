using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;

namespace Web.Api.Services.ReactionService
{
    public class ReactionService : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Reaction> _reactionRepo;

        public ReactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _reactionRepo = unitOfWork.GetBaseRepo<Reaction>();
        }

        public async Task<Reaction> Reaction(Guid userId, Guid ideaId, string reactionType)
        {
            try
            {
                var react = new Reaction();
                if (reactionType.Trim().ToLower() == "upvote")
                {
                    react = await CheckReact(1, ideaId, userId);
                    return react;
                }
                else if (reactionType.Trim().ToLower() == "downvote")
                {
                    react = await CheckReact(-1, ideaId, userId);
                    return react;
                }
                throw new Exception("Reaction type is wrong!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Reaction> CheckReact(int react, Guid ideaId, Guid userId)
        {
            try
            {
                var userReact = await _reactionRepo.Find(x => x.IdeaId == ideaId && x.UserId == userId);
                if (!userReact.Any())
                {
                    var result = _reactionRepo.Add(new Reaction
                    {
                        UserId = userId,
                        IdeaId = ideaId,
                        React = react
                    });
                    await _unitOfWork.CompleteAsync();
                    return result;
                }
                if (userReact.First().React == react || react != 1 && react != -1)
                {
                    var result = _reactionRepo.Delete(userReact.First().Id);
                    await _unitOfWork.CompleteAsync();
                    if (!result)
                    {
                        throw new Exception("Something wrong while deleting reaction");
                    }
                    return null;
                }
                if (userReact.First().React == -react)
                {
                    userReact.First().React = react;
                    var result = _reactionRepo.Update(userReact.First());
                    await _unitOfWork.CompleteAsync();
                    return result;
                }
                throw new Exception("Reaction type is wrong!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
