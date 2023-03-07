using Microsoft.AspNetCore.Http;

namespace Web.Api.DTOs.RequestModels
{
    public class FileRequestModel
    {
        public IFormFile file { get; set; }
    }
}
