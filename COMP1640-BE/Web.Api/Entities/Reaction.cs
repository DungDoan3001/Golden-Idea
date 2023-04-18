using System.ComponentModel.DataAnnotations;
using System;

namespace Web.Api.Entities
{
    public class Reaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int React { get; set; }

        public Guid UserId { get; set; }
        public Guid IdeaId { get; set; }

        public User User { get; set; }
        public Idea Idea { get; set; }
    }
}
