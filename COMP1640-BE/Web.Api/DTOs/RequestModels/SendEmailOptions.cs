using System.Collections.Generic;

namespace Web.Api.DTOs.RequestModels
{
    public class SendEmailOptions
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
