using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class RoleRequestModel
    {
        [Required(ErrorMessage = "Need to input name of the role")]
        public string Name { get; set; }
    }
}
