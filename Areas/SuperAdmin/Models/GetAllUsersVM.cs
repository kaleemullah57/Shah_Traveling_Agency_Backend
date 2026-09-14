namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class GetAllUsersVM
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? search { get; set; }
    }

    public class UserModel
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int UserTypeId { get; set; }
        public string? UserType { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? CreatedById { get; set; }
        public string? CreatdBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
