namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class BranchService
    {
    }



    // Add Branch Services
    public class AddBranchServiceModel
    {
        public int BranchId { get; set; }

        public int ServiceId { get; set; }

        public string BranchServiceName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

}
