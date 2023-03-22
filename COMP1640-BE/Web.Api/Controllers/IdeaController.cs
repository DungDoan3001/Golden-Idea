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
using Microsoft.AspNetCore.Http;
using System.Linq;
using Web.Api.Services.Topic;
using Web.Api.Services.Category;
using Web.Api.Services.User;
using Web.Api.Services.View;
using Web.Api.Services.Comment;
using Web.Api.Services.ReactionService;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Configuration;
using static Web.Api.Configuration.CacheKey;

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
        private readonly ITopicService _topicService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IViewService _viewService;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;
        private readonly IMemoryCache _cache;
        private IdeaCacheKey IdeaCacheKey = new IdeaCacheKey();

        public IdeaController(IMapper mapper, IIdeaService ideaService, IFileUploadService fileUploadService,
                            IFileService fileService, ITopicService topicService,
                            ICategoryService categoryService, IUserService userService, IViewService viewService,
                            ICommentService commentService, IReactionService reactionService, IMemoryCache cache)
        {
            _mapper = mapper;
            _ideaService = ideaService;
            _fileUploadService = fileUploadService;
            _fileService = fileService;
            _topicService = topicService;
            _categoryService = categoryService;
            _userService = userService;
            _viewService = viewService;
            _commentService = commentService;
            _reactionService = reactionService;
            _cache = cache;
        }

        /// <summary>
        /// Get all ideas.
        /// </summary>
        /// <returns>List of idea objects</returns>
        /// <response code="200">Successfully get all ideas</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<IdeaResponseModel>>> GetAll()
        {
            try
            {
                if (_cache.TryGetValue(IdeaCacheKey.GetAllCacheKey, out IEnumerable<IdeaResponseModel> ideaResponses)) { }
                else
                {
                    IEnumerable<Idea> ideas = await _ideaService.GetAllAsync();
                    ideaResponses = _mapper.Map<IEnumerable<IdeaResponseModel>>(ideas);
                    ideaResponses.ToList().ForEach(response =>
                    {
                        response.Files = null;
                    });
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(IdeaCacheKey.GetAllCacheKey, ideaResponses.OrderBy(x => x.Title), cacheEntryOptions);
                }
                return Ok(ideaResponses.OrderBy(x => x.Title));
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
        /// Get all ideas by user name.
        /// </summary>
        /// <returns>List of idea objects</returns>
        /// <response code="200">Successfully get all ideas</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("user/{userName}")]
        public async Task<ActionResult<IEnumerable<IdeaResponseModel>>> GetAllByAuthor([FromQuery] Guid topicId,[FromRoute] string userName)
        {
            try
            {
                IEnumerable<Idea> ideas = await _ideaService.GetAllByAuthorAsync(userName, topicId);
                IEnumerable<IdeaResponseModel>  IdeaResponses = _mapper.Map<IEnumerable<IdeaResponseModel>>(ideas);
                return Ok(IdeaResponses.OrderBy(x => x.Title));
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
        public async Task<ActionResult<IdeaResponseModel>> GetById([FromRoute] Guid ideaId)
        {
            try
            {
                Idea idea = await _ideaService.GetByIdAsync(ideaId);
                IdeaResponseModel IdeaResponse = _mapper.Map<IdeaResponseModel>(idea);
                return Ok(IdeaResponse);
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
        public async Task<ActionResult<IdeaResponseModel>> GetBySlug([FromRoute] string slug)
        {
            try
            {
                Idea idea = await _ideaService.GetBySlugAsync(slug);
                IdeaResponseModel IdeaResponse = _mapper.Map<IdeaResponseModel>(idea);
                return Ok(IdeaResponse);
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
        /// Get idea by topic
        /// </summary>
        /// <returns>List of Ideas</returns>
        /// <response code="200">Successfully get idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("topic/{topicId}")]
        public async Task<ActionResult<IdeaResponseModel>> GetByTopic([FromRoute] Guid topicId)
        {
            try
            {
                IEnumerable<Idea> ideas = await _ideaService.GetAllByTopicAsync(topicId);
                IEnumerable<IdeaResponseModel>  IdeaResponses = _mapper.Map<IEnumerable<IdeaResponseModel>>(ideas);
                return Ok(IdeaResponses);
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
        /// Create a idea
        /// </summary>
        /// <param name="requestModel">Request model for idea</param>
        /// <returns>A message if the creation is success.</returns>
        /// <response code="201">Successfully created the idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while create a idea</response>
        [HttpPost("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromForm] IdeaRequestModel requestModel)
        {
            try
            {
                // Check valid request model
                var message = await CheckValidRequest(requestModel, true);
                if(message != null)
                {
                    return StatusCode(message.StatusCode, message);
                }
                // Map Idea.
                Idea idea = _mapper.Map<Idea>(requestModel);
                // Get userId
                var user = await _userService.GetByUserName(requestModel.Username);
                idea.UserId = user.Id;
                // Upload thumbnail.
                if (requestModel.File != null)
                {
                    await UploadImage(requestModel.File, idea);
                }
                // Upload Files
                List<File> files = await UploadFileAsync(requestModel.ListFile, idea);
                // Auto update slug and LastUpdate time field.
                await UpdateSlugAndLastUpdateTime(idea);
                idea.CreatedAt = DateTime.UtcNow;

                // Create Idea.
                Idea createdIdea = await _ideaService.CreateAsync(idea);
                // Add files to Idea if any.
                if (files.Count > 0)
                {
                    IEnumerable<File> addedFiles = await _fileService.AddRangeAsync(files);
                }
                // Send email to owner of topic
                if(createdIdea != null)
                {
                    var topic = await _ideaService.SendEmailNotifyUserCreateIdea(createdIdea);
                }
                // Delete all idea cache
                var keyCache = IdeaCacheKey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(IdeaCacheKey));
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
        /// Search idea by title
        /// </summary>
        /// <param name="searchTerm">Title for searching</param>
        /// <returns>A message if the update is success.</returns>
        /// <response code="200">Successfully searched the idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while searched</response>
        [HttpGet("search")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<IdeaResponseModel>> SearchByTitle([FromQuery] string searchTerm)
        {
            try
            {
                var search = await _ideaService.SearchByTitle(searchTerm);
                var result = _mapper.Map<List<IdeaResponseModel>>(search);

                return Ok(result);
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
                // Check valid request model
                var message = await CheckValidRequest(requestModel, true);
                if (message != null)
                {
                    return StatusCode(message.StatusCode, message);
                }

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
                if(requestModel.File != null)
                {
                    await _fileUploadService.DeleteMediaAsync(idea.PublicId, true);
                }
                foreach(var file in idea.Files)
                {
                    if(file.Format == null)
                    {
                        await _fileUploadService.DeleteMediaAsync(file.PublicId, false);
                    } else await _fileUploadService.DeleteMediaAsync(file.PublicId, true);
                }
                await _fileService.DeleteRangeAsync(idea.Files);
                
                // Map Idea.
                _mapper.Map<IdeaRequestModel, Idea>(requestModel, idea);
                // Upload thumbnail.
                if (requestModel.File != null)
                {
                    await UploadImage(requestModel.File, idea);
                }
                // Upload Files
                List<File> files = await UploadFileAsync(requestModel.ListFile, idea);
                // Auto update slug and LastUpdate time field.
                await UpdateSlugAndLastUpdateTime(idea);
                // Create Idea.
                Idea updatedIdea = await _ideaService.UpdateAsync(idea);
                // Add files to Idea if any.
                if (files.Count > 0)
                {
                    IEnumerable<File> addedFiles = await _fileService.AddRangeAsync(files);
                }
                // Delete all idea cache
                var keyCache = IdeaCacheKey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(IdeaCacheKey));
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

        /// <summary>
        /// Delete a idea
        /// </summary>
        /// <param name="id">Id of the idea to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the idea</response>
        /// <response code="204">Successfully deleted the idea</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no idea with the given Id</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Idea idea = await _ideaService.GetByIdAsync(id);
                if(idea == null)
                {
                    return NotFound(new MessageResponseModel
                    {
                        Message = "Not Found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors= new List<string> {"Can not find the idea with the given id"}
                    });
                }

                // Delete Views, Comments, Reactions
                await _reactionService.DeleteByIdeaAsync(id);
                await _viewService.DeleteByIdeaAsync(id);
                await _commentService.DeleteByIdeaAsync(id);

                if (idea.IsFakeData == false)
                {
                    if (idea.PublicId != null) await _fileUploadService.DeleteMediaAsync(idea.PublicId, true);
                    foreach (var file in idea.Files)
                    {
                        if (file.Format == null)
                        {
                            await _fileUploadService.DeleteMediaAsync(file.PublicId, false);
                        }
                        else await _fileUploadService.DeleteMediaAsync(file.PublicId, true);
                    }
                    await _fileService.DeleteRangeAsync(idea.Files);
                }
                bool isDelete = await _ideaService.DeleteAsync(id);

                if (!isDelete)
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Not found",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors= new List<string> { "Can not detele an idea with the given id." }
                    });
                // Delete all idea cache
                var keyCache = IdeaCacheKey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(IdeaCacheKey));
                }
                return Ok(new MessageResponseModel
                {
                    Message = "Deleted",
                    StatusCode = (int)HttpStatusCode.OK
                });
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

        private async Task UploadImage(IFormFile image, Idea idea)
        {
            var uploadImageResult = await _fileUploadService.UploadImageAsync(image);
            idea.Image = uploadImageResult.SecureUrl.ToString();
            idea.PublicId = uploadImageResult.PublicId;
        }

        private async Task<MessageResponseModel> CheckValidRequest(IdeaRequestModel requestModel, bool isCheckClosureDate)
        {
            // Check if input topic is valid
            Topic topic = await _topicService.GetByIdAsync(requestModel.TopicId);
            if (topic == null)
            {
                return new MessageResponseModel
                {
                    Message = "Not Found",
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Errors = new List<string> { "Topic not found" }
                };
            }
            if (isCheckClosureDate)
            {
                // Check if topic still valid
                if (topic.ClosureDate < DateTime.UtcNow)
                {
                    return new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Can't post ideas because the topic submission deadline is over." }
                    };
                }
            }

            // Check if input category is valid
            Category category = await _categoryService.GetByIdAsync(requestModel.CategoryId);
            if (category == null)
            {
                return new MessageResponseModel
                {
                    Message = "Not Found",
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Errors = new List<string> { "Category not found." }
                };
            }

            // Check if input user is valid
            Entities.User user = await _userService.GetByUserName(requestModel.Username);
            if (user == null)
            {
                return new MessageResponseModel
                {
                    Message = "Not Found",
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Errors = new List<string> { "User not found." }
                };
            }

            return null;
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
            idea.IsFakeData = false;
            // Update Lastupdate field.
            idea.LastUpdate = DateTime.UtcNow;
        }

        private async Task<List<File>> UploadFileAsync(List<IdeaRequestModel_File> uploadFile, Idea idea)
        {
            List<File> files = new List<File>();
            // Adding linked File.
            if (uploadFile != null)
            {
                foreach (var file in uploadFile)
                {
                    var uploadFileResult = await _fileUploadService.UploadFileAsync(file.File);
                    File fileEntity = new File
                    {
                        FilePath = uploadFileResult.SecureUrl.ToString(),
                        PublicId = uploadFileResult.PublicId,
                        FileName = uploadFileResult.OriginalFilename,
                        Format = uploadFileResult.Format,
                        IdeaId = idea.Id
                    };
                    files.Add(fileEntity);
                }
            }
            return files;
        }
    }
}
