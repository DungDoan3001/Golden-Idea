using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.DTOs.RequestModels;
using CloudinaryDotNet.Actions;
using Web.Api.Services.FileUploadService;

namespace Web.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IFIleUploadService _fIleUploadService;

        public UploadFileController(IFIleUploadService fIleUploadService)
        {
            _fIleUploadService = fIleUploadService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] FileRequestModel fileInput)
        {
            try
            {
                //string url = await _uploadFileService.UploadFile(fileInput.file);
                RawUploadResult result = await _fIleUploadService.UploadImageAsync(fileInput.file);
                return Ok(result.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        [HttpPost("file")]
        public async Task<IActionResult> UploadFile([FromForm] FileRequestModel fileInput)
        {
            try
            {
                //string url = await _uploadFileService.UploadFile(fileInput.file);
                RawUploadResult result = await _fIleUploadService.UploadFileAsync(fileInput.file);
                return Ok(result.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}
