using System;

namespace Web.Api.DTOs.RequestModels
{
    public class ReactionRequestModel
    {
        public string Username { get; set; }
        public Guid IdeaId { get; set; }
    }
}
