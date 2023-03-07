using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.DTOs.RequestModels;
using CloudinaryDotNet.Actions;
using Web.Api.Services.FileUploadService;
using System.Collections.Generic;

namespace Web.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public UploadFileController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] FileRequestModel fileInput)
        {
            try
            {
                //string url = await _uploadFileService.UploadFile(fileInput.file);
                RawUploadResult result = await _fileUploadService.UploadImageAsync(fileInput.file);
                return Ok(result.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }

        [HttpPost("file")]
        public async Task<IActionResult> UploadFile([FromForm] FileRequestModel fileInput)
        {
            try
            {
                //string url = await _uploadFileService.UploadFile(fileInput.file);
                RawUploadResult result = await _fileUploadService.UploadFileAsync(fileInput.file);
                return Ok(result.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel
                {
                    Message = "Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string> { ex.GetBaseException().Message }
                });
            }
        }
    }
}
