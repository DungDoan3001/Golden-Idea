using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Services.FakeData;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataGenController : ControllerBase
    {
        private readonly IFakeDataService _fakeDataService;

        public FakeDataGenController(IFakeDataService fakeDataService)
        {
            _fakeDataService = fakeDataService;
        }

        /// <summary>
        /// Create fake data base on the number of data want to fake (number from 1 to 200)
        /// </summary>
        [HttpPost("{numberToGenerate}")]
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetAll([FromRoute] int numberToGenerate)
        {
            try
            {
                if(numberToGenerate < 0 && numberToGenerate > 200)
                {
                    return Conflict(new MessageResponseModel
                    {
                        Message = "Conflict",
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Errors = new List<string> { "Passed the limit of numberToGenerate" }
                    });
                }
                var result = await _fakeDataService.CreateFakeIdeaData(numberToGenerate);
                if (result)
                {
                    return Ok(new MessageResponseModel
                    {
                        Message = "Created successfully " + numberToGenerate + " accounts.",
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                return Conflict(new MessageResponseModel
                {
                    Message = "Conflict",
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Errors = new List<string> {"The result return false." }
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

        [HttpDelete("")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var result = await _fakeDataService.DeleteFakeDataAsync();
                if (result)
                {
                    return Ok(new MessageResponseModel
                    {
                        Message = "Deleted successfully every fake idea data",
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                return Conflict(new MessageResponseModel
                {
                    Message = "Conflict",
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Errors = new List<string> { "The result return false." }
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
    }
}
