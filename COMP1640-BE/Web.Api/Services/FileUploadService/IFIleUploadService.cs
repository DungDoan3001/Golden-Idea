using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task<DeletionResult> DeleteMediaAsync(string publicId, bool isImage);
        Task<RawUploadResult> UploadFileAsync(IFormFile file);
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    }
}