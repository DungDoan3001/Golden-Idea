using System.Threading.Tasks;
using System;

namespace Web.Api.Services.View
{
    public interface IViewService
    {
        Task<Entities.View> ViewCount(Guid ideaId, string username);
        Task<bool> DeleteByIdeaAsync(Guid ideaId);
    }
}