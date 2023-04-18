using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class TopicRequestModel
    {
        [Required(ErrorMessage = "Need to input name of the topic")]
        public string Name { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
        public string UserName { get; set; }
    }
}
