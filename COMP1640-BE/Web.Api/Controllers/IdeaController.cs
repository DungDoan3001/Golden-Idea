﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.IdeaService;
using Web.Api.Entities;
using AutoMapper;
using Web.Api.DTOs.RequestModels;
using Web.Api.Extensions;
using Web.Api.Services.FileUploadService;
using Web.Api.Services.FileService;
using Slugify;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Controllers
{
    [Route("api/ideas")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIdeaService _ideaService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IFileService _fileService;

        public IdeaController(IMapper mapper, IIdeaService ideaService, IFileUploadService fileUploadService, IFileService fileService)
        {
            _mapper = mapper;
            _ideaService = ideaService;
            _fileUploadService = fileUploadService;
            _fileService = fileService;
        }

        /// <summary>
        /// Get all ideas.
        /// </summary>
        /// <returns>List of idea objects</returns>
        /// <response code="200">Successfully get all ideas</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<IdeaResponeModel>>> GetAll()
        {
            try
            {
                IEnumerable<Idea> ideas = await _ideaService.GetAllAsync();
                IEnumerable<IdeaResponeModel> TopicResponses = _mapper.Map<IEnumerable<IdeaResponeModel>>(ideas);
                return Ok(TopicResponses);
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
        /// Get idea by Id
        /// </summary>
        /// <returns>an idea</returns>
        /// <response code="200">Successfully get idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("id/{ideaId}")]
        public async Task<ActionResult<IdeaResponeModel>> GetById([FromRoute] Guid ideaId)
        {
            try
            {
                Idea idea = await _ideaService.GetByIdAsync(ideaId);
                IdeaResponeModel TopicResponses = _mapper.Map<IdeaResponeModel>(idea);
                return Ok(TopicResponses);
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
        /// Get idea by slug
        /// </summary>
        /// <returns>an idea</returns>
        /// <response code="200">Successfully get idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<IdeaResponeModel>> GetBySlug([FromRoute] string slug)
        {
            try
            {
                Idea idea = await _ideaService.GetBySlugAsync(slug);
                IdeaResponeModel TopicResponses = _mapper.Map<IdeaResponeModel>(idea);
                return Ok(TopicResponses);
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
        /// Create a topic
        /// </summary>
        /// <param name="requestModel">Request model for topic</param>
        /// <returns>A message if the creation is success.</returns>
        /// <response code="201">Successfully created the topic</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a topic</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromForm] IdeaRequestModel requestModel)
        {
            try
            {
                // Map Idea.
                Idea idea = _mapper.Map<Idea>(requestModel);
                // Upload thumbnail.
                if (requestModel.Image != null)
                {
                    await UploadImage(requestModel.Image, idea);
                }
                // Upload Files
                List<File> files = await UploadFileAsync(requestModel.UploadFiles, idea);
                // Auto update slug and LastUpdate time field.
                await UpdateSlugAndLastUpdateTime(idea);

                // Create Idea.
                Idea createdIdea = await _ideaService.CreateAsync(idea);
                // Add files to Idea if any.
                if (files.Count > 0)
                {
                    IEnumerable<File> addedFiles = await _fileService.AddRangeAsync(files);
                }

                return Created(createdIdea.Id.ToString(), new MessageResponseModel { Message = "Success", StatusCode = (int)HttpStatusCode.Created });
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
        /// Update a idea
        /// </summary>
        /// <param name="requestModel">Request model for idea.</param>
        /// <param name="id">Id of the idea to be updated.</param>
        /// <returns>A message if the update is success.</returns>
        /// <response code="200">Successfully updated the idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while update a idea</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] IdeaRequestModel requestModel)
        {
            try
            {
                Idea idea = await _ideaService.GetByIdAsync(id);
                if (idea == null)
                {
                    return NotFound(new MessageResponseModel
                    {
                        Message = "Not Found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find idea with the given id." }
                    });
                }

                // Delete old media
                if(requestModel.Image != null)
                {
                    var isFileExisted = await _ideaService.CheckExistedImageContainDuplicateAsync(idea.Image);
                    if (!isFileExisted)
                    {
                        await _fileUploadService.DeleteMediaAsync(idea.PublicId);
                    }
                }
                foreach(var file in idea.Files)
                {
                    var isFileExisted = await _fileService.CheckExistedFilePathContainDuplicateAsync(file.FilePath);
                    if(!isFileExisted)
                    {
                        await _fileUploadService.DeleteMediaAsync(file.PublicId);
                    }
                    await _fileService.DeleteAsync(file.Id);
                }

                // Map Idea.
                _mapper.Map<IdeaRequestModel, Idea>(requestModel, idea);
                // Upload thumbnail.
                if (requestModel.Image != null)
                {
                    await UploadImage(requestModel.Image, idea);
                }
                // Upload Files
                List<File> files = await UploadFileAsync(requestModel.UploadFiles, idea);
                // Auto update slug and LastUpdate time field.
                await UpdateSlugAndLastUpdateTime(idea);
                // Create Idea.
                Idea updatedIdea = await _ideaService.UpdateAsync(idea);
                // Add files to Idea if any.
                if (files.Count > 0)
                {
                    IEnumerable<File> addedFiles = await _fileService.AddRangeAsync(files);
                }

                return Ok(new MessageResponseModel { Message = "Success", StatusCode = (int)HttpStatusCode.OK });
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

        private async Task UpdateSlugAndLastUpdateTime(Idea idea)
        {
            // Update auto populate field.
            // Create Slug.
            string slug = new SlugHelper().GenerateSlug(idea.Title);
            bool isDuplicateSlug = await _ideaService.CheckSlugExistedAsync(slug);
            if (isDuplicateSlug)
            {
                var random = new Random();
                slug += "-" + random.Next(1000, 9999);
            }
            idea.Slug = slug;
            // Update Lastupdate field.
            idea.LastUpdate = DateTime.UtcNow;
        }

        private async Task<List<File>> UploadFileAsync(List<IFormFile> uploadFile, Idea idea)
        {
            List<File> files = new List<File>();
            // Adding linked File.
            if (uploadFile != null)
            {
                foreach (var file in uploadFile)
                {
                    var uploadFileResult = await _fileUploadService.UploadFileAsync(file);
                    File fileEntity = new File
                    {
                        FilePath = uploadFileResult.SecureUrl.ToString(),
                        PublicId = uploadFileResult.PublicId,
                        IdeaId = idea.Id
                    };
                    files.Add(fileEntity);
                }
            }
            return files;
        }

        private async Task UploadImage(IFormFile image, Idea idea)
        {
            var uploadImageResult = await _fileUploadService.UploadImageAsync(image);
            idea.Image = uploadImageResult.SecureUrl.ToString();
            idea.PublicId = uploadImageResult.PublicId;
        }
    }
}