using System;
using System.Collections.Generic;

namespace Web.Api.DTOs.ResponseModels
{
    public class IdeaResponeModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastUpdate { get; set; }

        // Currently set to disable due to not finish view and reaction
        //public int UpVote { get; set; }
        //public int DownVote { get; set; }
        //public int View { get; set; }
        public IdeaResponeModel_User User { get; set; }
        public IdeaResponeModel_Topic Topic { get; set; }
        public IdeaResponeModel_Category Category { get; set; }
        public List<IdeaResponeModel_File> Files { get; set; }
    }

    public class IdeaResponeModel_User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class IdeaResponeModel_Topic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class IdeaResponeModel_Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class IdeaResponeModel_File
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtention { get; set; }
    }
}
