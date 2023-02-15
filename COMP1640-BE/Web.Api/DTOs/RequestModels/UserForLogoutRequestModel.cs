using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForLogoutRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
