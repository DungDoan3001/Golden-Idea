using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }
        /// <summary>
        /// Get all needed information for Contributors Chart.
        /// </summary>
        /// <response code="200">Successfully get all information</response>
        /// <response code="400">There is something wrong while execute.</response>
        [HttpGet("contributors")]
        public async Task<ActionResult<List<ContributorResponseModel>>> GetContributors() 
        {
            try
            {
                return await _chartService.GetContributor();
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

        [HttpGet("GetNumOfIdeaAnonyByDepartment")]
        public async Task<ActionResult<List<NumOfIdeaAnonyByDepartment>>> GetNumOfIdeaAnonyByDepartment()
        {
            var test = await _chartService.GetNumOfIdeaAnonyByDepartment();
            return null;
        }
    }
}
