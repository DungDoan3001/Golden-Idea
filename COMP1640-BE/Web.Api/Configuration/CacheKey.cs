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

    }
}
