namespace Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context
{
    public class PasswordService
    {
        public bool VerifyPassword(
            string password,
            string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                passwordHash
            );
        }
    }
}
