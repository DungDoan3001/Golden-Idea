using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

    }

}
