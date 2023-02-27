using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class UserForAuthenRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
