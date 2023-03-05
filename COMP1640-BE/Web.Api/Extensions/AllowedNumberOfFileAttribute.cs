using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Extensions
{ 
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowedNumberOfFileAttribute : Attribute, IActionFilter
    {
        private readonly int _numberOfFile;
        public AllowedNumberOfFileAttribute(int numberOfFile)
        {
            _numberOfFile = numberOfFile;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Form != null)
            {
                if(context.HttpContext.Request.Form.Files.Count > _numberOfFile)
                {
                    context.Result = new ObjectResult($"Maximum allowed number of file is {_numberOfFile}")
                    {
                        StatusCode = 413 //Payload Too Large
                    };
                }
            }
            
        }
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
