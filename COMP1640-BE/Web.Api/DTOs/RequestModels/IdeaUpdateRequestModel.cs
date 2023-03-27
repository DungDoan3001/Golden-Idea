using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.DTOs.RequestModels
{
    public class IdeaUpdateRequestModel : IdeaRequestModel
    {
        [BindProperty(Name = "OldListFile[]")]
        public List<string> OldListFile { get; set; }
    }
}
