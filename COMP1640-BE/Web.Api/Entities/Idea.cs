using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class Idea
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string PublicId { get; set; }
        public string Slug { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TopicId { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
        public Topic Topic { get; set; }

        public ICollection<File> Files { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
    }
}
