using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class Topic
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }

        public ICollection<Idea> Ideas { get; set; }
        public User User { get; set; }
    }
}
