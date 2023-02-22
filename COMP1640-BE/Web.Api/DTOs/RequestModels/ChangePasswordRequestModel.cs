namespace Web.Api.DTOs.RequestModels
{
    public class ChangePasswordRequestModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set;}
    }
}
