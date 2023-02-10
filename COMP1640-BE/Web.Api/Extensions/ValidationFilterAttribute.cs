using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Api.DTOs.ResponseModels;

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
                    StatusCode = 400,
                    Errors = new List<string>()
                };

                foreach (var state in context.ModelState)
                {
                    var Error = state.Value.Errors.Select(e => e.ErrorMessage);
                    errorModel.Errors = Error.ToList();
                }

                context.Result = new BadRequestObjectResult(errorModel);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }   
}
