using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Slugify;
using System.Threading.Tasks;

namespace Web.Api.Services.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly Cloudinary _cloudinary;
        private SlugHelper slugHelper = new SlugHelper();
        public FileUploadService(IConfiguration _configuration)
        {
            var acc = new Account
            (
                _configuration.GetSection("Cloudinary:CloudName").Value,
                _configuration.GetSection("Cloudinary:ApiKey").Value,
                _configuration.GetSection("Cloudinary:ApiSecret").Value
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            try
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(slugHelper.GenerateSlug(file.FileName.Trim().Split(".")[0]), stream),
                        PublicId = "GoldenIdeaImg/" + slugHelper.GenerateSlug(file.FileName.Trim()),
                        UniqueFilename = true,
                        UseFilenameAsDisplayName= true,
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                return uploadResult;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<RawUploadResult> UploadFileAsync(IFormFile file)
        {
            try
            {
                var uploadResult = new RawUploadResult();
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(slugHelper.GenerateSlug(file.FileName.Trim().Split(".")[0]), stream),
                        PublicId = "GoldenIdeaRaw/" + slugHelper.GenerateSlug(file.FileName.Trim()),
                        UniqueFilename = true,
                        UseFilenameAsDisplayName = true,
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                return uploadResult;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<DeletionResult> DeleteMediaAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
