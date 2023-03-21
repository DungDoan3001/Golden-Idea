using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.Chart;

namespace Web.Api.Controllers
{
    [Route("api/charts")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        public readonly IChartService _chartService;
        private readonly IMemoryCache _cache;
        public ChartController(IChartService chartService, IMemoryCache cache)
        {
            _chartService = chartService;
            _cache = cache;
        }
        /// <summary>
        /// Get all needed information for Contributors Chart.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("contributors-by-department")]
        public async Task<ActionResult<List<ContributorResponseModel>>> GetContributorsByDepartment() 
        {
            try
            {
                return await _chartService.GetContributorByDepart();
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
        /// Total Idea count for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("ideas-by-department")]
        public async Task<ActionResult<List<TotalIdeaOfDepartmentsResponseModel>>> GetTotalIdeaByDepartment()
        {
            try
            {
                return await _chartService.GetTotalIdeaOfEachDepartment();
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
        /// Get List of Ideas for dashboard.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("list-of-ideas")]
        public async Task<ActionResult<List<IdeaForChartResponseModel>>> GetIdeasForChart()
        {
            try
            {
                return await _chartService.GetIdeasForChart();
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
        /// Percentage Idea count for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("percentage-of-ideas-by-department")]
        public async Task<ActionResult<List<PercentageOfIdeaForEachDepartment>>> GetPercentageOfIdeasByDepartment()
        {
            try
            {
                return await _chartService.GetPercentageOfIdeaForEachDepartments();
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
        /// Total staff and idea and comment and topic count.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("get-staff-idea-comment-topic")]
        public async Task<ActionResult<TotalStaffAndIdeaAndTopicAndCommentResponseModel>> GetTotalStaffAndIdeaAndCommentAndTopic()
        {
            try
            {
                return await _chartService.GetTotalOfStaffAndIdeaAndTopicAndCommment();
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
        /// Get the number of ideas with anonymous and idea with no comments for each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("num-of-ideas-anonymous-by-department")]
        public async Task<ActionResult<List<NumOfIdeaAnonyByDepartment>>> GetNumOfIdeaAnonyAndNoCommentByDepart()
        {
            try
            {
                return await _chartService.GetNumOfIdeaAnonyAndNoCommentByDepart();
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
        /// Get the number of comments with anonymous and non-anonymous from users in each department.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("num-of-comment-by-department")]
        public async Task<ActionResult<List<NumOfCommentResponseModel>>> GetNumOfCommentByDepart()
        {
            try
            {
                return await _chartService.GetNumOfCommentByDepart();
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
        /// Get the total of comments and ideas in 3 months by day.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("daily-report-in-three-months")]
        public async Task<ActionResult<List<DailyReportResponseModel>>> GetDailyReportInThreeMonths()
        {
            try
            {
                return await _chartService.GetDailyReportInThreeMonths();
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
