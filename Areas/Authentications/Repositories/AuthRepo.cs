using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.Authentications.Models;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.Authentications.Repositories
{
    public class AuthRepo
    {
        private readonly DapperContext _dapperContext;
        public AuthRepo(DapperContext context)
        {
            _dapperContext = context;
        }

        #region Register Users

        public async Task<(bool Success, string Message)> RegisterUser(RegisterUserRequest request)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var parameters = new DynamicParameters();

                parameters.Add("@UserName", request.UserName);
                parameters.Add("@Email", request.Email);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@BranchId", request.BranchId);
                parameters.Add("@UserTypeId", request.UserTypeId);

                parameters.Add(
                    "@Message",
                    dbType: DbType.String,
                    size: -1,
                    direction: ParameterDirection.Output
                );

                parameters.Add(
                    "@ReturnValue",
                    dbType: DbType.Int32,
                    direction: ParameterDirection.ReturnValue
                );

                await connection.ExecuteAsync(
                    "[Account].[Register_Users]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int returnValue = parameters.Get<int>("@ReturnValue");

                string message =
                    parameters.Get<string>("@Message") ?? string.Empty;

                return returnValue switch
                {
                    3 => (true, message),
                    1 => (false, message),
                    2 => (false, message),
                    _ => (false, message)
                };
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        #endregion

        #region Login Users

        // Login Users
        public async Task<LoginUser?> Login(LoginRequest request)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Email", request.Email);

            var user = await connection.QueryFirstOrDefaultAsync<LoginUser>(
                "[Account].[User_Login]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return user;
        }
        #endregion
    }
}
