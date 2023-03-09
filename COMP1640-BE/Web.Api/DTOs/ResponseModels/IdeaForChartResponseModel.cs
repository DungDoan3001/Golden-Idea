using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class IdeaForChartResponseModel
    {
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalView { get; set; }
        public int TotalComment { get; set; }
        public int TotalUpVote { get; set; }
        public int TotalDownVote { get; set; }
    }
}
