using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;

namespace Web.Api.Services.Chart
{
    public interface IChartService
    {
        Task<List<ContributorResponseModel>> GetContributorByDepart();
        Task<List<NumOfIdeaAnonyByDepartment>> GetNumOfIdeaAnonyAndNoCommentByDepart();
        Task<List<NumOfCommentResponseModel>> GetNumOfCommentByDepart();
        Task<List<TotalIdeaOfDepartmentsResponseModel>> GetTotalIdeaOfEachDepartment();
        Task<TotalStaffAndIdeaAndTopicAndCommentResponseModel> GetTotalOfStaffAndIdeaAndTopicAndCommment();
        Task<List<PercentageOfIdeaForEachDepartment>> GetPercentageOfIdeaForEachDepartments();
        Task<List<IdeaForChartResponseModel>> GetIdeasForChart();
        Task<List<DailyReportResponseModel>> GetDailyReportInThreeMonths();
    }
}