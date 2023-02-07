using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
