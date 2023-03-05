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
                        File = new FileDescription(file.FileName, stream),
                        //PublicId = "GoldenIdeaImg/" + slugHelper.GenerateSlug(file.FileName.Trim().ToLower()),
                        UniqueFilename = true,
                        AssetFolder = "GoldenIdeaImg",
                        UseAssetFolderAsPublicIdPrefix = true,
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
                        File = new FileDescription(file.FileName, stream),
                        UniqueFilename = true,
                        AssetFolder = "GoldenIdeaRaw",
                        UseAssetFolderAsPublicIdPrefix = true,
                        UseFilename = true,
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

        public async Task<DeletionResult> DeleteMediaAsync(string publicId, bool isImage)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);

                // Check if the param is img or raw.
                if (isImage)
                {
                    deleteParams.ResourceType = ResourceType.Image;
                } else
                {
                    deleteParams.ResourceType = ResourceType.Raw;
                }
                
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
