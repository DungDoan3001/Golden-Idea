using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Services.UploadFileService
{
    public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile file);
    }
}