using System.ComponentModel.DataAnnotations;

namespace Web.Api.DTOs.RequestModels
{
    public class DepartmentRequestModel
    {
        [Required(ErrorMessage = "Need to input name of the department")]
        public string Name { get; set; }
    }
}
