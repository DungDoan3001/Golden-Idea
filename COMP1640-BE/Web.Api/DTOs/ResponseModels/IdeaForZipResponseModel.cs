using System.Collections.Generic;
using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class IdeaForZipResponseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int View { get; set; }
        public string User_Email { get; set; }
        public string Category_name { get; set; }
        public string Topic { get; set;}
    }
}
