using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Models;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Repositories
{
    public class SuperAdminSetupRepo
    {
        private readonly DapperContext _dapperContext;
        public SuperAdminSetupRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        #region Branches

        // Get Branches
        public async Task<(int StatusCode, string Message, List<GetBranchModel> Data)> GetAllBranchesAsync(BranchVM vm, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Search", vm.Search, DbType.String, size: 200);
                parameters.Add("@PageNumber", vm.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", vm.PageSize, DbType.Int32);
                parameters.Add("@UserID", userId, DbType.Int32);
                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = (await connection.QueryAsync<GetBranchModel>(
                    "[Data].[Get_All_Branches]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                int returnValue = parameters.Get<int>("@ReturnValue");
                string message = parameters.Get<string>("@Message") ?? "Unknown response.";

                return (returnValue, message, data);
            }
            catch (Exception ex)
            {
                return (
                    0,
                    $"An unexpected error occurred while fetching branches: {ex.Message}",
                    new List<GetBranchModel>()
                );
            }
        }



        // Save Branches
        public async Task<(int StatusCode, string Message)> AddBranchAsync(AddBranchModel model, int CreatedById)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@BranchName", model.BranchName, DbType.String, size: 200);

                parameters.Add("@Location", model.Location, DbType.String, size: 300);

                parameters.Add("@IsActive", model.IsActive, DbType.Boolean);

                parameters.Add("@CreatedById", CreatedById, DbType.Int32);

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                // Capture stored procedure RETURN value
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "[Data].[Add_Branches]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int returnValue = parameters.Get<int>("@ReturnValue");

                string message = parameters.Get<string>("@Message") ?? "Unknown response.";

                return (returnValue, message);
            }
            catch (Exception ex)
            {
                return (
                    0,
                    $"An unexpected error occurred while adding the branch: {ex.Message}"
                );
            }
        }
        #endregion
    }

}
