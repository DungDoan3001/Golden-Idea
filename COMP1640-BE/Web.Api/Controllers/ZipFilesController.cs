using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System;
using System.Collections;
using System.Linq;
using CsvHelper;
using Web.Api.Services.ZipFile;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipFilesController : ControllerBase
    {
        private readonly IZipFileService _zipFileService;
        public ZipFilesController(IZipFileService zipFileService)
        {
            _zipFileService = zipFileService;
        }
        [HttpGet("{topicId}")]
        public async Task<IActionResult> DownloadDeliveriesForToday([FromRoute] Guid topicId)
        {
            var zipFile = await _zipFileService.ZipIdeasOfTopicExpired(topicId);
            return File(zipFile.Bytes, "application/octet-stream", zipFile.FileName);
        }
    }
}
