using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class GetUserReactionResponseModel
    {
        public string Email { get; set; }
        public int React { get; set; }
        public Guid IdeaId { get; set; }
    }
}
