using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class CategoryRequestModel
    {
        [Required(ErrorMessage = "Need to input name of the category")]
        public string Name { get; set; }
    }
}
