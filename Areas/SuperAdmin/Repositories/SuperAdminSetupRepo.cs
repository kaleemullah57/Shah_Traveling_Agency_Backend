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

        #region Countries

        // Get Countries
        public async Task<(int ReturnValue, string Message, List<Country> Data)> GetCountriesAsync(CountryVM vm, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", vm.search, DbType.String);

            parameters.Add("@PageNumber", vm.pageNumber, DbType.Int32);

            parameters.Add("@PageSize", vm.pageSize, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = (await connection.QueryAsync<Country>(
                "Data.Sp_Get_Countries",
                parameters,
                commandType: CommandType.StoredProcedure
            )).ToList();

            var returnValue = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message, data);
        }



        // Add Countries
        public async Task<(int ReturnValue, string Message)> AddCountryAsync(AddCountryRequest model, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CountryName", model.CountryName);
            parameters.Add("@CountryCode", model.CountryCode);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedById", userId);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.Sp_Add_Countries",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message);
        }
        #endregion

        #region Provices


        // Add Provinces
        public async Task<(bool Success, int StatusCode, string Message)> AddProvince(AddProvinceModel model, int createdById)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@provincename", model.ProvinceName);
            parameters.Add("@Countryid", model.CountryId);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedByid", createdById);
            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "[Data].[Sp_Add_Provinces]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var returnValue = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? "Unknown error";

            return returnValue switch
            {
                3 => (true, 200, message),
                1 => (false, 403, message),
                2 => (false, 409, message),
                4 => (false, 400, message),
                _ => (false, 500, message)
            };
        }



        // Get Provinces List
        public async Task<(int ReturnValue, string Message, int TotalCount, IEnumerable<ProvinceModel> Data)> GetProvincesAsync(GetProvincesRequest vm, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Search", vm.Search);
                parameters.Add("@UserID", userId);
                parameters.Add("@PageNumber", vm.PageNumber);
                parameters.Add("@PageSize", vm.PageSize);

                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = await connection.QueryAsync<ProvinceModel>(
                    "[Data].[Sp_Get_Provinces]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@ReturnValue");
                string message = parameters.Get<string>("@Message") ?? string.Empty;
                int totalCount = parameters.Get<int>("@TotalCount");

                return (returnValue, message, totalCount, data);
            }
            catch (Exception ex)
            {
                return (0, ex.Message, 0, Enumerable.Empty<ProvinceModel>());
            }
        }
        #endregion

        #region Cities

        // Add Cities
        public async Task<(bool Success, int StatusCode, string Message)> AddCity(AddCityModel model, int createdById)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CityName", model.CityName);
            parameters.Add("@CountryId", model.CountryId);
            parameters.Add("@ProvinceId", model.ProvinceId);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedById", createdById);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "[Data].[SP_Add_Cities]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var returnValue = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? "Unknown error";

            return returnValue switch
            {
                3 => (true, 200, message),

                1 => (false, 403, message),

                2 => (false, 409, message),

                4 => (false, 400, message),

                _ => (false, 500, message)
            };
        }



        // Get Cities List
        public async Task<(bool Success, int StatusCode, string Message, IEnumerable<CityResponseModel> Data, int TotalCount)> GetCities(GetCitiesRequestModel model, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", string.IsNullOrWhiteSpace(model.Search) ? null : model.Search.Trim());

            parameters.Add("@UserID", userId);
            parameters.Add("@PageNumber", model.PageNumber);
            parameters.Add("@PageSize", model.PageSize);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var multi = await connection.QueryMultipleAsync(
                "[Data].[Sp_Get_Cities]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var data = await multi.ReadAsync<CityResponseModel>();

            var totalCount = parameters.Get<int>("@TotalCount");

            var message =
                parameters.Get<string>("@Message")
                ?? string.Empty;

            var returnValue =
                parameters.Get<int>("@ReturnValue");

            return returnValue switch
            {
                2 => (true, 200, message, data, totalCount),

                1 => (false, 403, message, Enumerable.Empty<CityResponseModel>(), 0),

                _ => (false, 500, message, Enumerable.Empty<CityResponseModel>(), 0)
            };
        }
        #endregion

        #region Post Types


        // Add Post Types
        public async Task<(int ReturnValue, string Message)> AddPostTypeAsync(AddPostTypeModel model, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@PostTypeName", model.PostTypeName);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "[Travel].[Sp_Add_PostTypes]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@ReturnValue");
                string message = parameters.Get<string>("@Message") ?? string.Empty;

                return (returnValue, message);
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }

        #endregion
    }

}
