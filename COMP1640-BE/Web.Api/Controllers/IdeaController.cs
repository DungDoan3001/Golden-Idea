using System.Collections.Generic;
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
        /// <returns>A topic just created</returns>
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
                    var uploadImageResult = await _fileUploadService.UploadImageAsync(requestModel.Image);
                    idea.Image = uploadImageResult.SecureUrl.ToString();
                    idea.PublicId = uploadImageResult.PublicId;
                }

                List<File> files = new List<File>();
                // Adding linked File.
                if (requestModel.UploadFiles != null)
                {
                    foreach (var file in requestModel.UploadFiles)
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

                // Update auto populate field.
                    // Create Slug.
                string slug = new SlugHelper().GenerateSlug(idea.Title);
                bool isDuplicateSlug = await _ideaService.CheckSlugExitAsync(slug);
                if(isDuplicateSlug)
                {
                    var random = new Random();
                    slug += "-" + random.Next(1000, 9999);
                }
                idea.Slug = slug;
                    // Update Lastupdate field.
                idea.LastUpdate = DateTime.UtcNow;

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
    }
}
