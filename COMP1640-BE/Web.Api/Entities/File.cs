using System.ComponentModel.DataAnnotations;
using System;

namespace Web.Api.Entities
{
    public class File
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FilePath { get; set; }
        public string PublicId { get; set; }

        public Guid IdeaId { get; set; }
        public Idea Idea { get; set; }

    }
}
