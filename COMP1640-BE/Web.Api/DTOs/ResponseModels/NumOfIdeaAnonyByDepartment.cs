namespace Web.Api.DTOs.ResponseModels
{
    public class NumOfIdeaAnonyByDepartment
    {
        public string DepartmentName { get; set; }
        public int TotalIdea { get; set; }
        public int TotalAnonymous { get; set; }
        public int TotalNoAnonymous { get; set; }
    }
}
