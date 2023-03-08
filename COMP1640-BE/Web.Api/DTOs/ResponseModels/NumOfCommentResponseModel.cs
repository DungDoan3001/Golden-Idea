namespace Web.Api.DTOs.ResponseModels
{
    public class NumOfCommentResponseModel
    {
        public string DepartmentName { get; set; }
        public int CommentAnonymous { get; set; }
        public int CommentNonAnonymous { get; set; }
    }
}
