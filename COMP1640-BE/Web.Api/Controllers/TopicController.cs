using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Topic;
using System.Linq;
using Web.Api.Extensions;
using Web.Api.Services.User;
using Web.Api.Entities;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Web.Api.Configuration.CacheKey;

namespace Web.Api.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private TopicCacheKey topicCachekey = new TopicCacheKey();
        public TopicController(IMapper mapper, ITopicService topicService, IUserService userService, IMemoryCache cache)
        {
            _mapper = mapper;
            _topicService = topicService;
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// Get all topics.
        /// </summary>
        /// <returns>List of Topic objects</returns>
        /// <response code="200">Successfully get all topics</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<TopicResponseModel>>> GetAll()
        {
            try
            {
                if(_cache.TryGetValue(topicCachekey.GetAllCacheKey,out IEnumerable<TopicResponseModel> TopicResponses)) { }
                else
                {
                    IEnumerable<Entities.Topic> topics = await _topicService.GetAllAsync();
                    TopicResponses = _mapper.Map<IEnumerable<TopicResponseModel>>(topics);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(topicCachekey.GetAllCacheKey, TopicResponses.OrderByDescending(x => x.ClosureDate), cacheEntryOptions);
                }
                return Ok(TopicResponses.OrderByDescending(x => x.ClosureDate));

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
        /// Get all topics that user has posted idea in by UserName.
        /// </summary>
        /// <returns>List of Topic objects belong to UserName</returns>
        /// <response code="200">Successfully get all topics</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("user/{userName}")]
        public async Task<ActionResult<IEnumerable<TopicResponseModel>>> GetAllByUserId([FromRoute] string userName)
        {
            try
            {
                IEnumerable<Entities.Topic> topics = await _topicService.GetAllByUserName(userName);
                IEnumerable<TopicResponseModel>  TopicResponses = _mapper.Map<IEnumerable<TopicResponseModel>>(topics);
                return Ok(TopicResponses.OrderByDescending(x => x.ClosureDate));
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
        /// Get a topic by Id.
        /// </summary>
        /// <param name="id">Id of the topic</param>
        /// <returns>A topic with the given Id</returns>
        /// <response code="200">Successfully get the topic with the given Id</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no topic with the given Id</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicResponseModel>> GetById([FromRoute] Guid id)
        {
            try
            {
                Entities.Topic topic = await _topicService.GetByIdAsync(id);
                if (topic == null)
                {
                    return NotFound(new MessageResponseModel
                    {
                        Message = "Not found.",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find the topic with the given id." }
                    });
                }
                TopicResponseModel TopicResponse = _mapper.Map<TopicResponseModel>(topic);

                return Ok(TopicResponse);
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
        public async Task<ActionResult<TopicRequestModel>> Create([FromBody] TopicRequestModel requestModel)
        {
            try
            {
                bool check = await CheckExist(requestModel.Name);
                if (check)
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "The name already existed." }
                    });

                Entities.Topic topic = _mapper.Map<Entities.Topic>(requestModel);

                // Get User
                User user = await _userService.GetByUserName(requestModel.UserName);
                if(user == null)
                {
                    return NotFound(new MessageResponseModel
                    {
                        Message= "Error",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find user with given user name"}
                    });
                } else topic.UserId = user.Id;

                Entities.Topic createdTopic = await _topicService.CreateAsync(topic);

                if (createdTopic == null)
                    return Conflict(new MessageResponseModel 
                    { 
                        Message = "Conflict", 
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors= new List<string> { "Error while create new." }
                    });
                // Delete all topic cache
                var keyCache = topicCachekey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(topicCachekey));
                }
                
                return Created(createdTopic.Id.ToString(), _mapper.Map<TopicResponseModel>(createdTopic));
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
        /// Update a topic
        /// </summary>
        /// <param name="requestModel">Request model for topic.</param>
        /// <param name="id">Id of the topic to be updated.</param>
        /// <returns>A topic just updated</returns>
        /// <response code="200">Successfully updated the topic</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="409">There is a conflict while update a topic</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<TopicResponseModel>> Update([FromRoute] Guid id, [FromBody] TopicRequestModel requestModel)
        {
            try
            {
                Entities.Topic topic = await _topicService.GetByIdAsync(id);
                if(topic.Name.Trim().ToLower() != requestModel.Name.Trim().ToLower())
                {
                    bool check = await CheckExist(requestModel.Name);
                    if (check)
                        return Conflict(new MessageResponseModel
                        {
                            Message = "Conflict",
                            StatusCode = (int)HttpStatusCode.Conflict,
                            Errors = new List<string> { "The name already existed." }
                        });
                }
                if (topic == null) 
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find the topic with the given Id."}
                    });

                _mapper.Map<TopicRequestModel, Entities.Topic>(requestModel, topic);
                // Get User
                User user = await _userService.GetByUserName(requestModel.UserName);
                if (user == null)
                {
                    return NotFound(new MessageResponseModel
                    {
                        Message = "Error",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find user with given user name" }
                    });
                }
                else topic.UserId = user.Id;

                Entities.Topic updatedTopic = await _topicService.UpdateAsync(topic);
                // Delete all topic cache
                var keyCache = topicCachekey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(topicCachekey));
                }
                return Ok(_mapper.Map<TopicResponseModel>(updatedTopic));
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
        /// Delete a topic
        /// </summary>
        /// <param name="id">Id of the topic to be deleted.</param>
        /// <returns>null</returns>
        /// <response code="200">Successfully deleted the topic</response>
        /// <response code="204">Successfully deleted the topic</response>
        /// <response code="400">There is something wrong while execute.</response>
        /// <response code="404">There is no topic with the given Id</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                Entities.Topic topic = await _topicService.GetByIdAsync(id);

                if (topic == null)
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors = new List<string> { "Can not find the topic with the given id."}
                    });

                if(topic.Ideas.Count() > 0)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Sorry! We cannot delete the department because it contains some ideas." }
                    });
                }

                bool isDelete = await _topicService.DeleteAsync(id);
                if (!isDelete)
                    return NotFound(new MessageResponseModel 
                    { 
                        Message = "Not Found.", 
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Errors= new List<string> { "Error while delete." }
                    });
                // Delete all topic cache
                var keyCache = topicCachekey.GetType().GetProperties();
                foreach (var key in keyCache)
                {
                    _cache.Remove(key.GetValue(topicCachekey));
                }
                return NoContent();
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

        private async Task<bool> CheckExist(string name)
        {
            IEnumerable<Entities.Topic> checkTopics = await _topicService.GetByNameAsync(name);
            if (checkTopics.Any())
                return true;
            return false;
        }
    }
}
