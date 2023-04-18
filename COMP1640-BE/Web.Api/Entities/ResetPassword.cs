using System.ComponentModel.DataAnnotations;
using System;

namespace Web.Api.Entities
{
    public class ResetPassword
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}
