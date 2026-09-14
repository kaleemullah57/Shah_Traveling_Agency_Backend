namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class BranchService
    {
    }



    // Add Branch Services
    public class AddBranchServiceModel
    {
        public int ServiceId { get; set; }

        public string BranchServiceName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }




    // Get Branch Servies
    public class BranchServicesRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class BranchServiceModel
    {
        public int BranchServiceId { get; set; }
        public string BranchServiceName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int? CreatedById { get; set; }
        public string UserName { get; set; } = string.Empty;
    }







    // Update Branch Services
    public class UpdateBranchServiceRequest
    {
        public int BranchServiceId { get; set; }
        public int ServiceId { get; set; }
        public bool IsActive { get; set; } = true;
        public string BranchServiceName { get; set; } = string.Empty;
    }
}
