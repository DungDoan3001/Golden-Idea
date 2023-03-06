using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class TopicResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Username { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
    }
}
