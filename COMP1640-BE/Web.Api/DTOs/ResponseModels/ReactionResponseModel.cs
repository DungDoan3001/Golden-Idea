using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class ReactionResponseModel
    {
        public Guid Id { get; set; }
        public int React { get; set; }
        public Guid UserId { get; set; }
        public Guid IdeaId { get; set; }
    }
}
