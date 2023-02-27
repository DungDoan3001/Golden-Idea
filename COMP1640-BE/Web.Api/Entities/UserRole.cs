using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Web.Api.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
        
    }
}
