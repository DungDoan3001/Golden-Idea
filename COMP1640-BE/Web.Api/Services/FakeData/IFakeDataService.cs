using System.Threading.Tasks;

namespace Web.Api.Services.FakeData
{
    public interface IFakeDataService
    {
        Task<bool> CreateFakeIdeaData(int numberOfIdeaToCreate);
        Task<bool> DeleteFakeDataAsync();
    }
}