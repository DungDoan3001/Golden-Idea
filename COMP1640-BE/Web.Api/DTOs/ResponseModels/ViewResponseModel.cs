using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class ViewResponseModel
    {
        public Guid Id { get; set; }
        public int VisitTime { get; set; }
        public Guid UserId { get; set; }
        public Guid IdeaId { get; set; }
    }
}
