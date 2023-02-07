using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public ICollection<Idea> Ideas { get; set; }
    }
}
