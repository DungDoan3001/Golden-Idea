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
        /// <summary>
        /// Upload an image to cloudinary
        /// </summary>
        /// <param name="fileInput">Request model for upload image</param>
        /// <returns>An image just uploaded</returns>
        /// <response code="201">Successfully upload an image</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while uploaded image</response>
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
        
        /// <summary>
        /// Upload multiple files to cloudinary
        /// </summary>
        /// <param name="fileInput">Request model for upload files</param>
        /// <returns>An image just uploaded</returns>
        /// <response code="201">Successfully upload files</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while uploaded files</response>
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
