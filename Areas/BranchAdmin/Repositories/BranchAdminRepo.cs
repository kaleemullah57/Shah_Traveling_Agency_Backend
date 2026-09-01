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

        public async Task<(int ReturnValue, string Message)> AddDestination(AddDestinationModel model, List<string>? picturePaths, int createdById, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@DestinationName", model.DestinationName);
            parameters.Add("@Description", model.Description);
            parameters.Add("@CountryId", model.CountryId);
            parameters.Add("@ProvinceId", model.ProvinceId);
            parameters.Add("@CityId", model.CityId);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedById", createdById);
            parameters.Add("@Branchid", branchId);

            // Multiple paths as JSON
            string? picturePathJson = picturePaths != null &&
                                      picturePaths.Any()
                ? System.Text.Json.JsonSerializer.Serialize(picturePaths)
                : null;

            parameters.Add("@PicturePath", picturePathJson);

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



        // Get Branch Destinations
        public async Task<(int ReturnValue, string Message, List<DestinationModel> Data)> GetDestinationsByBranchId(int branchId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var result = await connection.QueryAsync<DestinationDbModel>(
                "Travel.Sp_Get_Destinations_By_BranchId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            var destinations = result.Select(x => new DestinationModel
            {
                DestinationId = x.DestinationId,
                DestinationName = x.DestinationName,
                Description = x.Description,

                CountryId = x.CountryId,
                CountryName = x.CountryName,

                ProvinceId = x.ProvinceId,
                ProvinceName = x.ProvinceName,

                CityId = x.CityId,
                CityName = x.CityName,

                PicturePath = string.IsNullOrWhiteSpace(x.PicturePath)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(x.PicturePath)
                      ?? new List<string>(),

                CreatedById = x.CreatedById,
                CreatedBy = x.CreatedBy,

                CreatedOn = x.CreatedOn,
                IsActive = x.IsActive
            }).ToList();

            return (returnValue, message, destinations);
        }

        private class DestinationDbModel
        {
            public int DestinationId { get; set; }

            public string DestinationName { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;

            public int CountryId { get; set; }

            public string CountryName { get; set; } = string.Empty;

            public int? ProvinceId { get; set; }

            public string? ProvinceName { get; set; }

            public int? CityId { get; set; }

            public string? CityName { get; set; }

            public string? PicturePath { get; set; }

            public int CreatedById { get; set; }

            public string CreatedBy { get; set; } = string.Empty;

            public DateTime CreatedOn { get; set; }

            public bool IsActive { get; set; }
        }


        #endregion
    }
}
