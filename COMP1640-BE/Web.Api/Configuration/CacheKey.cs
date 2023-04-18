using System.Collections.Generic;

namespace Web.Api.Configuration
{
    public class CacheKey
    {
        public List<string> TopicCacheKey { get; set; } = new List<string>();
        public List<string> UserCacheKey { get; set; } = new List<string>();
        public List<string> DepartmentCacheKey { get; set; } = new List<string>();
        public List<string> IdeaCacheKey { get; set; } = new List<string>();
        public List<string> CategoryCacheKey { get; set; } = new List<string>();
        public string NumOfIdeaAnonyAndNoCommentByDepartCacheKey { get; set; } = "NumOfIdeaAnonyAndNoCommentByDepartCacheKey";
        public string NumOfCommentByDepartCacheKey { get; set; } = "NumOfCommentByDepartCacheKey";
        public string PercentageOfIdeasByDepartmentCacheKey { get; set; } = "PercentageOfIdeasByDepartmentCacheKey";
        public string TotalStaffAndIdeaAndCommentAndTopicCacheKey { get; set; } = "TotalStaffAndIdeaAndCommentAndTopicCacheKey";
        public string TotalIdeaOfEachDepartmentCacheKey { get; set; } = "TotalIdeaOfEachDepartmentCacheKey";
        public string DailyReportInThreeMonthsCacheKey { get; set; } = "DailyReportInThreeMonthsCacheKey";
        

    }
}
