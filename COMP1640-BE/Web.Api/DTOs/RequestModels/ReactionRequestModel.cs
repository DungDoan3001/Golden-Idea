using System;

namespace Web.Api.DTOs.RequestModels
{
    public class ReactionRequestModel
    {
        public Guid UserId { get; set; }
        public Guid IdeaId { get; set; }
    }
}
