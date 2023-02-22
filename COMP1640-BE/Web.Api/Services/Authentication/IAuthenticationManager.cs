using System.Threading.Tasks;
using Web.Api.DTOs.RequestModels;

namespace Web.Api.Services.Authentication
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenRequestModel userForAuth);
        Task<string> CreateToken();
        Task<bool> GenerateChangePasswordTokenAsync(Entities.User user);
    }
}
