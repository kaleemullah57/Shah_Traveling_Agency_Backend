using Dapper;
using Microsoft.Data.SqlClient;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Models;
using System.Data;
using System.Data.Common;

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



        // Get Branch By Id
        public async Task<(int ReturnValue, string Message, GetBranchByIdModel? Data)> GetBranchByBranchId(int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", branchId);
            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);
            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryFirstOrDefaultAsync<GetBranchByIdModel>(
                "Data.SP_Get_BranchByBranchId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var returnValue = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message, data);
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




        // Get Post Types

        public async Task<(int ReturnValue, string Message, IEnumerable<TravelTypeModel> Data)> GetTravelTypesAsync(GetTravelTypeRequest vm, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Search", vm.Search);
                parameters.Add("@PageNumber", vm.PageNumber);
                parameters.Add("@PageSize", vm.PageSize);
                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                using var multi = await connection.QueryMultipleAsync(
                    "[Travel].[Sp_Get_TravelTypes]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // First result set: SELECT * FROM Travel.PostTypes
                await multi.ReadAsync();

                // Second result set: required data
                var data = await multi.ReadAsync<TravelTypeModel>();

                int returnValue = parameters.Get<int>("@ReturnValue");
                string message = parameters.Get<string>("@Message") ?? string.Empty;

                return (returnValue, message, data);
            }
            catch (Exception ex)
            {
                return (0, ex.Message, Enumerable.Empty<TravelTypeModel>());
            }
        }

        #endregion

        #region Branch Logos

        // Add Branch Logo
        public async Task<(int ReturnValue, string Message)> AddBranchLogoAsync(AddBranchLogoModel model, string logoPath, string originalFileName, string fileExtension, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@BranchId", model.BranchId);
                parameters.Add("@LogoPath", logoPath);
                parameters.Add("@OriginalFileName", originalFileName);
                parameters.Add("@FileExtension", fileExtension);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@CreatedById", userId);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "[Data].[SP_BranchLogo_Insert]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@ReturnValue");

                string message =
                    parameters.Get<string>("@Message") ?? string.Empty;

                return (returnValue, message);
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }



        // Get Branch Logo
        public async Task<(int ReturnValue, string Message, BranchLogoModel? Data)> GetBranchLogoAsync(int UserID)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@UserID", UserID);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = await connection.QueryFirstOrDefaultAsync<BranchLogoModel>(
                    "[Data].[Sp_Get_BranchLogo]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@ReturnValue");

                string message = parameters.Get<string>("@Message") ?? string.Empty;

                return (returnValue, message, data);
            }
            catch (Exception ex)
            {
                return (0, ex.Message, null);
            }
        }
        #endregion

        #region Post Categories


        // Add Post Category
        public async Task<(int ReturnValue, string Message)> AddPostCategoryAsync(AddPostCategoryModel model, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Categoryname", model.CategoryName);
                parameters.Add("@Description", model.Description);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "[Travel].[Sp_Add_PostCategories]",
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



        // Get Post Categories
        public async Task<(int ReturnValue, string Message, int TotalCount, IEnumerable<GetPostCategoryModel> Data)> GetPostCategoriesAsync(GetPostCategoryRequest vm, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Search", vm.Search);
                parameters.Add("@PageNumber", vm.PageNumber);
                parameters.Add("@PageSize", vm.PageSize);
                parameters.Add("@UserID", userId);

                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = await connection.QueryAsync<GetPostCategoryModel>(
                    "[Travel].[Sp_Get_PostCategories]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@ReturnValue");

                string message = parameters.Get<string>("@Message") ?? string.Empty;

                int totalCount = parameters.Get<int>("@TotalCount");

                return (
                    returnValue,
                    message,
                    totalCount,
                    data
                );
            }
            catch (Exception ex)
            {
                return (
                    0,
                    ex.Message,
                    0,
                    Enumerable.Empty<GetPostCategoryModel>()
                );
            }
        }
        #endregion

        #region Airlines

        // Add Airlines
        public async Task<(int ReturnValue, string Message)> AddAirlineAsync(AddAirlineRequest model, int createdById)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AirlineName", model.AirlineName, DbType.String);

            parameters.Add("@AirlineCode", model.AirlineCode, DbType.String);

            parameters.Add("@IATACode", model.IATACode, DbType.String);

            parameters.Add("@ICAOCode", model.ICAOCode, DbType.String);

            parameters.Add("@CountryId", model.CountryId, DbType.Int32);

            parameters.Add("@LogoPath", model.LogoPath, DbType.String);

            parameters.Add("@IsActive", model.IsActive, DbType.Boolean);

            parameters.Add("@CreatedById", createdById, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "[Data].[Sp_Add_Airlines]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message);
        }





        // Get Airlines
        public async Task<(IEnumerable<AirlineModel> Data, int TotalCount, int ReturnValue, string Message)> GetAirlinesAsync(AirlineVM vm, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", vm.Search, DbType.String);

            parameters.Add("@PageNumber", vm.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", vm.PageSize, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync<AirlineModel>(
                "[Data].[Sp_Get_Airlines]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var totalCount = parameters.Get<int?>("@TotalCount") ?? 0;

            var returnValue = parameters.Get<int?>("@ReturnValue") ?? 0;

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (
                data,
                totalCount,
                returnValue,
                message
            );
        }









        // Edit Airlines
        public async Task<(int StatusCode, string Message)> EditAirline(EditAirlineRequest vm, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AirlineId", vm.AirlineId, DbType.Int32);

            parameters.Add("@AirlineName", vm.AirlineName, DbType.String, size: 200);

            parameters.Add("@AirlineCode", vm.AirlineCode, DbType.String, size: 200);

            parameters.Add("@IATACode", vm.IATACode, DbType.String, size: 200);

            parameters.Add("@ICAOCode", vm.ICAOCode, DbType.String, size: 200);

            parameters.Add("@CountryId", vm.CountryId, DbType.Int32);

            parameters.Add("@IsActive", vm.IsActive, DbType.Boolean);

            parameters.Add("@UserId", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            await connection.ExecuteAsync(
                "Data.Sp_Edit_Airlines",
                parameters,
                commandType: CommandType.StoredProcedure);


            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";


            return (statusCode, message);
        }





        // Delete Airlines
        public async Task<(int StatusCode, string Message)> DeleteAirline(int airlineId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AirlineId", airlineId, DbType.Int32);
            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.Sp_Delete_Airlines",
                parameters,
                commandType: CommandType.StoredProcedure);

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (statusCode, message);
        }


        #endregion

        #region Airports

        public async Task<(int StatusCode, string Message)> AddAirportAsync(AddAirportRequest request, int userId)
        {

            var parameters = new DynamicParameters();

            parameters.Add("@AirportName", request.AirportName);
            parameters.Add("@IataCode", request.IataCode);
            parameters.Add("@IcaoCode", request.IcaoCode);
            parameters.Add("@CountryId", request.CountryId);
            parameters.Add("@ProvinceId", request.ProvinceId);
            parameters.Add("@CityId", request.CityId);
            parameters.Add("@isInternational", request.IsInternational);
            parameters.Add("@IsActive", request.IsActive);
            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();
            await connection.ExecuteAsync(
                "Data.SP_Add_Airports",
                parameters,
                commandType: CommandType.StoredProcedure);

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (statusCode, message);
        }





        // Get Airports
        public async Task<(List<AirportModel> Data, int TotalCount, int StatusCode, string Message)> GetAirportsAsync(AirportListRequest request, int userId)
        {

            var parameters = new DynamicParameters();

            parameters.Add("@Search", string.IsNullOrWhiteSpace(request.Search) ? null : request.Search);

            parameters.Add("@PageNumber", request.PageNumber);
            parameters.Add("@PageSize", request.PageSize);
            parameters.Add("@UserID", userId);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();
            var data = (
                await connection.QueryAsync<AirportModel>(
                    "Data.SP_Get_Airports",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )
            ).ToList();

            int totalCount = parameters.Get<int>("@TotalCount");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            int statusCode = parameters.Get<int>("@ReturnValue");

            return (
                data,
                totalCount,
                statusCode,
                message
            );
        }





        // Update Airports
        public async Task<(int StatusCode, string Message)> UpdateAirportAsync(UpdateAirportRequest request, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AirportId", request.AirportId);
            parameters.Add("@AirportName", request.AirportName);
            parameters.Add("@IataCode", request.IataCode);
            parameters.Add("@IcaoCode", request.IcaoCode);
            parameters.Add("@CountryId", request.CountryId);
            parameters.Add("@ProviceId", request.ProvinceId);
            parameters.Add("@CityId", request.CityId);
            parameters.Add("@IsInternational", request.IsInternational);
            parameters.Add("@IsActive", request.IsActive);
            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.SP_Update_Airports",
                parameters,
                commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (
                statusCode,
                message
            );
        }




        // Delete Airports
        public async Task<(int StatusCode, string Message)> DeleteAirportAsync(int airportId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AirportId", airportId);
            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.SP_Delete_Airports",
                parameters,
                commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (
                statusCode,
                message
            );
        }
        #endregion

        #region Services


        // Add Services
        public async Task<(int StatusCode, string Message)> AddService(AddServiceRequest model, int createdById)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ServiceName", model.ServiceName);
            parameters.Add("@Description", model.Description);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedByID", createdById);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.Sp_Add_Services",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (returnValue, message);
        }





        // Get Services
        public async Task<(int StatusCode, string Message, List<ServiceModel> Data, int TotalCount)> GetServices(GetServicesRequest vm, int userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Search", vm.Search, DbType.String, size: 200);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@PageNumber", vm.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", vm.PageSize, DbType.Int32);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            using var connection = _dapperContext.CreateConnection();
            var data = (
                await connection.QueryAsync<ServiceModel>(
                    "Data.Sp_Get_Services_By_SuperAdmin",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )
            ).ToList();


            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            int totalCount = parameters.Get<int>("@TotalCount");


            return (
                statusCode,
                message,
                data,
                totalCount
            );
        }






        // Delete Services
        public async Task<(int StatusCode, string Message)> DeleteService(int serviceId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ServiceId", serviceId, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Data.SP_Delete_Services",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (
                statusCode,
                message
            );
        }






        // Edit Services
        public async Task<(int StatusCode, string Message)> EditService(EditServiceRequest vm, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ServiceId", vm.ServiceId, DbType.Int32);

            parameters.Add("@ServiceName", vm.ServiceName, DbType.String, size: 200);

            parameters.Add("@Description", vm.Description, DbType.String);

            parameters.Add("@IsActive", vm.IsActive, DbType.Boolean);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            await connection.ExecuteAsync(
                "Data.Sp_Edit_Services",
                parameters,
                commandType: CommandType.StoredProcedure
            );


            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";


            return (
                statusCode,
                message
            );
        }
        #endregion

        #region Get All Users
        public async Task<(List<UserModel> Data, int TotalCount, int StatusCode, string Message)> GetUsersAsync(GetAllUsersVM vm, int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Search", vm.search);
                parameters.Add("@PageNumber", vm.PageNumber);
                parameters.Add("@PageSize", vm.PageSize);
                parameters.Add("@UserID", userId);

                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = (await connection.QueryAsync<UserModel>(
                    "Data.Sp_Get_Users",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                var totalCount = parameters.Get<int?>("@TotalCount") ?? 0;

                var message = parameters.Get<string>("@Message") ?? string.Empty;

                var statusCode = parameters.Get<int?>("@ReturnValue") ?? 0;

                return (
                    data,
                    totalCount,
                    statusCode,
                    message
                );
            }
            catch (Exception ex)
            {
                return (
                    new List<UserModel>(),
                    0,
                    0,
                    ex.Message
                );
            }
        }
        #endregion

        #region Passenger Types

        // Get Passengers Types
        public async Task<(int ReturnCode, string Message, List<PassengerTypeModel> Data)> GetPassengerTypesAsync(int userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = (await _db.QueryAsync<PassengerTypeModel>(
                "[Data].[SP_Get_PassengerTypes]",
                parameters,
                commandType: CommandType.StoredProcedure))
                .ToList();

            var returnCode = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnCode, message, data);
        }







        // Add Passengers Types

        public async Task<(int ReturnCode, string Message)> AddPassengerTypeAsync(AddPassengerTypeRequest request, int userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@PassengerTypeName", request.PassengerTypeName);

            parameters.Add("@IsActive", request.IsActive);

            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _db.ExecuteAsync(
                "[Data].[SP_Add_PassengerType]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnCode = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnCode, message);
        }






        // Update Passenger Types
        public async Task<(int ReturnCode, string Message)> UpdatePassengerTypeAsync(UpdatePassengerTypeRequest request, int userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@PassengerTypeId", request.PassengerTypeId);
            parameters.Add("@PassengerTypeName", request.PassengerTypeName);
            parameters.Add("@IsActive", request.IsActive);
            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _db.ExecuteAsync(
                "[Data].[SP_Update_PassengerType]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnCode = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnCode, message);
        }
        #endregion
    }

}
