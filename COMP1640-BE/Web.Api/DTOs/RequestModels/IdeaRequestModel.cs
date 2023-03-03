using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Web.Api.DTOs.RequestModels
{
    public class IdeaRequestModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TopicId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
