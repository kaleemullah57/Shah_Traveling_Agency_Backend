namespace Shah_Traveling_Agency_API.Areas.Authentications.Models
{
    public class LoginVM
    {
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public int BranchId { get; set; }
    }

    public class LoginResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public int BranchId { get; set; }
        public string Token { get; set; }
    }
}
