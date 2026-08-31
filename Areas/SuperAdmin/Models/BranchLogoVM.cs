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



    // Get Branch Logo

  
    public class BranchLogoModel
    {
        public int BranchLogoId { get; set; }
        public string LogoPath { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}

