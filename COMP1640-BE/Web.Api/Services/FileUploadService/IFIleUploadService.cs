using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Services.FileUploadService
{
    public interface IFIleUploadService
    {
        Task<DeletionResult> DeleteMediaAsync(string publicId);
        Task<RawUploadResult> UploadFileAsync(IFormFile file);
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    }
}