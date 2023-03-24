using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Web.Api.Services.ZipFile;
using System.Threading.Tasks;
using System.Net;
using Web.Api.DTOs.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Extensions;
using Web.Api.Entities.Configuration;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ZipFilesController : ControllerBase
    {
        private readonly IZipFileService _zipFileService;
        public ZipFilesController(IZipFileService zipFileService)
        {
            _zipFileService = zipFileService;
        }

        /// <summary>
        /// Zip all file csv which include all ideas of the topic.
        /// </summary>
        /// <returns>File zip include all ideas of the topic.</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("download-all-ideas-of-topic/{topicId}")]
        [Roles(IdentityRoles.QAManager)] // Roles Here
        public async Task<IActionResult> DownloadAllIdeasOfTopic([FromRoute] Guid topicId)
        {
            try
            {
                var zipFile = await _zipFileService.ZipIdeasOfTopicExpired(topicId);
                return File(zipFile.Bytes, "application/octet-stream", zipFile.FileName);
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
        /// Zip all data of dashboard.
        /// </summary>
        /// <returns>File zip include all data of dashboard.</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("download-data-dashboard")]
        [Roles(IdentityRoles.QAManager)] // Roles Here
        public async Task<IActionResult> DownloadDataDashboard()
        {
            try
            {
                var zipFile = await _zipFileService.ZipDashboardData();
                return File(zipFile.Bytes, "application/octet-stream", zipFile.FileName);
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
