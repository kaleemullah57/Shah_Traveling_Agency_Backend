namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class BranchLogoVM
    {
    }


    // Add Branch Logo
    public class AddBranchLogoModel
    {
        public int BranchId { get; set; }

        public IFormFile Logo { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}

