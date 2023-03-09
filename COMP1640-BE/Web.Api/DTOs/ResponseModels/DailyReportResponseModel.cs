using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class DailyReportResponseModel
    {
        public string Date { get; set; }
        public int TotalIdea { get; set; }
        public int TotalComment { get; set; }
    }
}
