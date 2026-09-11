namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class ServiceVM
    {
    }
    public class GetServicesRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class ServiceModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
    }




    public class AddServiceRequest
    {
        public string ServiceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
