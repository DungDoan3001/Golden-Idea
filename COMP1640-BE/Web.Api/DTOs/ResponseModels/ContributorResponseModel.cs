namespace Web.Api.DTOs.ResponseModels
{
    public class ContributorResponseModel
    {
        public string DepartmentName { get; set; }
        public int TotalUser { get; set; }
        public int Contributor { get; set; }
        public int NonContributor { get; set; }

    }
}
