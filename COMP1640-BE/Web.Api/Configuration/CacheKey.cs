namespace Web.Api.Configuration
{
    public static class CacheKey
    {
        public class TopicCacheKey
        {
            public string GetAllCacheKey { get; set; } = "GetAllTopicCacheKey";
            public string GetByUserIdCacheKey { get; set; } = "GetTopicByUserIdCacheKey";
            public string GetByIdCacheKey { get; set; } = "GetTopicByIdCacheKey";
        }
        public class UserCacheKey
        {
            public string GetAllCacheKey { get; set; } = "GetAllUserCacheKey";
            public string GetAllStaffCacheKey { get; set; } = "GetAllStaffCacheKey";
            public string GetAllAdminQACacheKey { get; set; } = "GetAllAdminQACacheKey";
            public string GetByIdCacheKey { get; set; } = "GetUserByIdCacheKey";
        }
        public class CategoryCacheKey
        {
            public string GetAllCacheKey { get; set; } = "GetAllCategoryCacheKey";
            public string GetByIdCacheKey { get; set; } = "GetByIdCategoryCacheKey";
        }
        public class DepartmentCacheKey
        {
            public string GetAllCacheKey { get; set; } = "GetAllDepartmentCacheKey";
            public string GetByIdCacheKey { get; set; } = "GetByIdDepartmentCacheKey";
        }
        public class IdeaCacheKey
        {
            public string GetAllCacheKey { get; set; } = "GetAllIdeaCacheKey";
            public string GetAllByAuthorCacheKey { get; set; } = "GetAllIdeaByAuthorCacheKey";
            public string GetByIdCacheKey { get; set; } = "GetIdeaByIdCacheKey";
            public string GetBySlugCacheKey { get; set; } = "GetIdeaBySlugCacheKey";
            public string GetByTopicCacheKey { get; set; } = "GetIdeaByTopicCacheKey";
        }
    }
}
