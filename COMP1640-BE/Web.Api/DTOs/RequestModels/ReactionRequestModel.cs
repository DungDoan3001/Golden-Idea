using System;

namespace Web.Api.DTOs.RequestModels
{
    public class ReactionRequestModel
    {
        public Guid userId { get; set; }
        public Guid ideaId { get; set; }
    }
}
