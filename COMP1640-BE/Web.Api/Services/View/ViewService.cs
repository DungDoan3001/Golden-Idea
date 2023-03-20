using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.Data.Queries;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;

namespace Web.Api.Services.View
{
    public class ViewService : IViewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Entities.User> _userManager;
        private readonly IGenericRepository<Entities.View> _viewRepo;
        protected AppDbContext _context;
        private readonly IViewQuery _viewQuery;

        public ViewService(IUnitOfWork unitOfWork, UserManager<Entities.User> userManager, AppDbContext context, IViewQuery viewQuery)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _viewRepo = unitOfWork.GetBaseRepo<Entities.View>();
            _context = context;
            _viewQuery = viewQuery;
        }

        public async Task<Entities.View> ViewCount(Guid ideaId, string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new Exception("Can not find the user!");
                }
                var idea = await _context.Ideas.FindAsync(ideaId);
                if (idea == null)
                {
                    throw new Exception("Can not find the idea!");
                }
                var userView = await _viewRepo.Find(x => x.IdeaId == ideaId && x.UserId == user.Id);
                if (userView.Any())
                {
                    userView.First().VisitTime = userView.First().VisitTime + 1;
                    var result = _viewRepo.Update(userView.First());
                    await _unitOfWork.CompleteAsync();
                    return result;
                }
                var addView = _viewRepo.Add(new Entities.View
                {
                    IdeaId = idea.Id,
                    UserId = user.Id,
                    VisitTime = 1
                });
                await _unitOfWork.CompleteAsync();
                return addView;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteByIdeaAsync(Guid ideaId)
        {
            try
            {
                var views = await _viewQuery.GetAllByIdeaAsync(ideaId);
                if(views.Any())
                {
                    _viewRepo.DeleteRange(views);
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }      
    }
}
