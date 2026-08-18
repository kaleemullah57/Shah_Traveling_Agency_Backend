namespace Shah_Traveling_Agency_API.Areas.Authentications.Models
{
    public class RegisterUserVM
    {
    }


    public class RegisterUserRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int BranchId { get; set; }

        public int UserTypeId { get; set; }
    }
}
