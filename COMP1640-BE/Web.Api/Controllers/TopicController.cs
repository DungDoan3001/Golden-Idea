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

namespace Web.Api.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITopicService _topicService;

        public TopicController(IMapper mapper, ITopicService topicService)
        {
            _mapper = mapper;
            _topicService = topicService;
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
                IEnumerable<Entities.Topic> topics = await _topicService.GetAllAsync();
                IEnumerable<TopicResponseModel> TopicResponses = _mapper.Map<IEnumerable<TopicResponseModel>>(topics);
                return Ok(TopicResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        /// <summary>
        /// Get all topics by UserId.
        /// </summary>
        /// <returns>List of Topic objects belong to UserId</returns>
        /// <response code="200">Successfully get all topics</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TopicResponseModel>>> GetAllByUserId([FromRoute] Guid userId)
        {
            try
            {
                IEnumerable<Entities.Topic> topics = await _topicService.GetAllByUserId(userId);
                IEnumerable<TopicResponseModel> TopicResponses = _mapper.Map<IEnumerable<TopicResponseModel>>(topics);
                return Ok(TopicResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
                    return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound });
                }
                TopicResponseModel topicRespone = _mapper.Map<TopicResponseModel>(topic);
                return Ok(topicRespone);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
                    return Conflict(new MessageResponseModel { Message = "The name already existed", StatusCode = (int)HttpStatusCode.Conflict });
                Entities.Topic topic = _mapper.Map<Entities.Topic>(requestModel);
                Entities.Topic createdTopic = await _topicService.CreateAsync(topic);
                if (createdTopic == null)
                    return Conflict(new MessageResponseModel { Message = "Error while create new.", StatusCode = (int)HttpStatusCode.Conflict });
                return Created(createdTopic.Id.ToString(), _mapper.Map<TopicResponseModel>(createdTopic));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
                bool check = await CheckExist(requestModel.Name);
                if (check)
                    return Conflict(new MessageResponseModel { Message = "The name already existed", StatusCode = (int)HttpStatusCode.Conflict });
                Entities.Topic topic = await _topicService.GetByIdAsync(id);
                if (topic == null) return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound });
                _mapper.Map<TopicRequestModel, Entities.Topic>(requestModel, topic);
                Entities.Topic updatedTopic = await _topicService.UpdateAsync(topic);
                return Ok(_mapper.Map<TopicResponseModel>(updatedTopic));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
                if (topic == null) return NotFound(new MessageResponseModel { Message = "Not found.", StatusCode = (int)HttpStatusCode.NotFound });
                bool isDelete = await _topicService.DeleteAsync(id);
                if (!isDelete)
                    return NotFound(new MessageResponseModel { Message = "Error while update.", StatusCode = (int)HttpStatusCode.NotFound });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponseModel { Message = ex.GetBaseException().Message, StatusCode = (int)HttpStatusCode.BadRequest });
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
