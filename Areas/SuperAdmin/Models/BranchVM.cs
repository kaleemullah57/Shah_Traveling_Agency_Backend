namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class BranchVM
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


    // Get Branches
    public class GetBranchModel
    {
        public string BranchName { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public int CreatedById { get; set; }

        public string UserName { get; set; } = string.Empty;
    }



    // Add Branches
    public class AddBranchModel
    {
        public string BranchName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
