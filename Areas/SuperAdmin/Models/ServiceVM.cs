namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class ServiceVM
    {
    }




    public class AddServiceRequest
    {
        public string ServiceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
