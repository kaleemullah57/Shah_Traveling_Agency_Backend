namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class BranchServicesVM
    {
    }



    public class GetBranchServicesRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class BranchServiceResponse
    {
        public string BranchServiceName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
