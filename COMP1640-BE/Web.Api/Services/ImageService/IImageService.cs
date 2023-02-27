using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Web.Api.Services.ImageService
{
    public interface IImageService
    {
        Task<ImageUploadResult> AddImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}