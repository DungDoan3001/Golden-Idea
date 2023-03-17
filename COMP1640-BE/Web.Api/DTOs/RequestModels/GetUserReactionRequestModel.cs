using System;

namespace Web.Api.DTOs.RequestModels
{
    public class GetUserReactionRequestModel
    {
        public Guid IdeaId { get; set; }
        public string Username { get; set; }
    }
}
