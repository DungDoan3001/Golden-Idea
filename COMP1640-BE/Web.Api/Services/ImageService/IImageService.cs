using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Web.Api.Services.ImageService
{
    public interface IImageService
    {
        Task<RawUploadResult> UploadFileAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}