using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.Chart
{
    public interface IChartService
    {
        Task<List<ContributorResponseModel>> GetContributor();
        Task<List<NumOfIdeaAnonyByDepartment>> GetNumOfIdeaAnonyByDepartment();
    }
}