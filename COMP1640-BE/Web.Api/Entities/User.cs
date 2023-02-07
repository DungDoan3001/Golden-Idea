using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Guid RoleId { get; set; }
        public Guid DepartmentId { get; set; }

        public Role Role { get; set; }
        public Department Department { get; set; }

        public ICollection<Idea> Ideas { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
    }
}
