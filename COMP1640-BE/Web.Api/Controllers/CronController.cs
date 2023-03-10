using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Data.Context;
using System.Linq;
using Web.Api.DTOs.ResponseModels;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Web.Api.Controllers
{
    [Route("api/cron")]
    [ApiController]
    public class CronController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CronController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Cron()
        {
            try
            {
                var data = await _context.Roles.Take(1).ToListAsync();
                return Ok(new MessageResponseModel
                {
                    Message = true.ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                });
            }
            catch (System.Exception)
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
