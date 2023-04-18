using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class GetUserReactionResponseModel
    {
        public string Username { get; set; }
        public int React { get; set; }
        public Guid IdeaId { get; set; }
    }
}
