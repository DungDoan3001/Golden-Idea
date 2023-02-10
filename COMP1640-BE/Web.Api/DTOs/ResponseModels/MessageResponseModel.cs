using System.Collections.Generic;

namespace Web.Api.DTOs.ResponseModels
{
    public class MessageResponseModel
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
    }
}
