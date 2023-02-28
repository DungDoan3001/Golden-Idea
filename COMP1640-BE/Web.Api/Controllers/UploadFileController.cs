using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.UploadFileService;
using Web.Api.DTOs.RequestModels;
using Web.Api.Services.ImageService;
using CloudinaryDotNet.Actions;

namespace Web.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly IImageService _imageService;

        public UploadFileController(IUploadFileService uploadFileService, IImageService imageService)
        {
            _uploadFileService = uploadFileService;
            _imageService = imageService;
        }

        [HttpPost("")]
        public async Task<IActionResult> GetAGtell([FromForm] FileRequestModel fileInput)
        {
            try
            {
                //string url = await _uploadFileService.UploadFile(fileInput.file);
                RawUploadResult result = await _imageService.UploadFileAsync(fileInput.file);
                return Ok(result.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
