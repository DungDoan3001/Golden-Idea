using System;
using System.Collections.Generic;

namespace Web.Api.DTOs.ResponseModels
{
    public class IdeaResponseModel
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
        public IdeaResponseModel_User User { get; set; }
        public IdeaResponseModel_Topic Topic { get; set; }
        public IdeaResponseModel_Category Category { get; set; }
        public List<IdeaResponseModel_File> Files { get; set; }
    }

    public class IdeaResponseModel_User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }

    public class IdeaResponseModel_Topic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
    }

    public class IdeaResponseModel_Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class IdeaResponseModel_File
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtention { get; set; }
    }
}
