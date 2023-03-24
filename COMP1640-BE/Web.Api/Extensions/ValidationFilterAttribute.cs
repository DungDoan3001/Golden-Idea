using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Api.DTOs.ResponseModels;
using System.Net;

namespace Web.Api.Extensions
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorModel = new MessageResponseModel
                {
                    Message = "Validation Error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = new List<string>()
                };

                foreach (var state in context.ModelState)
                {
                    if (state.Value.Errors.Select(e => e.ErrorMessage).Any())
                    {
                        var err = state.Value.Errors.Select(e => e.ErrorMessage);
                        errorModel.Errors.AddRange(err);
                    }
                }

                context.Result = new BadRequestObjectResult(errorModel);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }   
}
