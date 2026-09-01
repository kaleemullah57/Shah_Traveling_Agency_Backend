using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using System.Data;
using System.Text.Json;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories
{
    public class BranchAdminRepo
    {
        private readonly DapperContext _dapperContext;
        public BranchAdminRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        #region Destinations

        public async Task<(int ReturnValue, string Message)> AddDestination(AddDestinationModel model, int createdById, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@DestinationName", model.DestinationName, DbType.String);

            // Multiple picture paths → JSON string
            var picturePathJson = model.PicturePath != null &&
                                  model.PicturePath.Any()
                ? JsonSerializer.Serialize(model.PicturePath)
                : null;

            parameters.Add("@PicturePath", picturePathJson, DbType.String);

            parameters.Add("@CountryId", model.CountryId, DbType.Int32);

            parameters.Add("@ProvinceId", model.ProvinceId, DbType.Int32);

            parameters.Add("@CityId", model.CityId, DbType.Int32);

            parameters.Add("@IsActive", model.IsActive, DbType.Boolean);

            parameters.Add("@CreatedById", createdById, DbType.Int32);

            parameters.Add("@Branchid", branchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Travel.Sp_Add_Destinations",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message);
        }
        #endregion
    }
}
