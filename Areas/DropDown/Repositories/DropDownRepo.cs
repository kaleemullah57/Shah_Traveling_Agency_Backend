using Dapper;
using Microsoft.Data.SqlClient;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Models;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.DropDown.Repositories
{
    public class DropDownRepo
    {
        private readonly DapperContext _dapperContext;
        public DropDownRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }




        #region Countries DropDown
        public async Task<IEnumerable<dynamic>> GetCountries()
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync("DropDown.Sp_Countries",
                commandType: CommandType.StoredProcedure);
            return result;
        }

        #endregion

        #region Provices
        public async Task<(IEnumerable<dynamic> Data, string? Message, int ReturnValue)> GetProvincesByCountryIdAsync(int countryId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CountryID", countryId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync(
                "DropDown.Sp_Get_Provicnes_by_CountryId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var message = parameters.Get<string?>("@Message");

            var returnValue = parameters.Get<int>("@ReturnValue");

            return (
                data,
                message,
                returnValue
            );
        }
        #endregion

        #region Cities
        public async Task<(IEnumerable<dynamic> Data, string? Message, int ReturnValue)> GetCitiesByProvinceIdAsync(int provinceId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ProvinceId", provinceId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync(
                "DropDown.Sp_Get_Cities_By_ProvinceID",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var message = parameters.Get<string?>("@Message");

            var returnValue = parameters.Get<int>("@ReturnValue");

            return (
                data,
                message,
                returnValue
            );
        }
        #endregion

        #region Services

        public async Task<(int StatusCode, string Message, IEnumerable<dynamic> Data)> GetServices(string? search)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", string.IsNullOrWhiteSpace(search) ? null : search, DbType.String, size: 200);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync(
                "DropDown.Sp_Services",
                parameters,
                commandType: CommandType.StoredProcedure);

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (
                statusCode,
                message,
                data
            );
        }


        #endregion

        #region Branches

        public async Task<(int StatusCode, string Message, IEnumerable<dynamic> Data)> GetBranchesAsync(int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                var data = await connection.QueryAsync(
                    "DropDown.Sp_Get_Branches",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var message = parameters.Get<string>("@Message") ?? "";

                return (
                    data.Any() ? 2 : 0,
                    message,
                    data
                );
            }
            catch (Exception ex)
            {
                return (
                    0,
                    ex.Message,
                    Enumerable.Empty<dynamic>()
                );
            }
        }


        #endregion

        #region User Types

        public async Task<(int StatusCode, string Message, IEnumerable<dynamic> Data)> GetUserTypesAsync(int userId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                var data = await connection.QueryAsync(
                    "DropDown.Sp_Get_UserTypes",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var message = parameters.Get<string>("@Message") ?? string.Empty;

                return (
                    data.Any() ? 2 : 0,
                    message,
                    data
                );
            }
            catch (Exception ex)
            {
                return (
                    0,
                    ex.Message,
                    Enumerable.Empty<dynamic>()
                );
            }
        }
        #endregion

        #region Airlines

        public async Task<(int StatusCode, string Message, IEnumerable<dynamic>? Data)> GetAirlinesAsync()
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = await connection.QueryAsync(
                    "DropDown.Sp_Get_Airlines",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var returnValue = parameters.Get<int>("@ReturnValue");
                var message = parameters.Get<string>("@Message") ?? string.Empty;

                return (
                    returnValue,
                    message,
                    data
                );
            }
            catch (Exception ex)
            {
                return (
                    0,
                    ex.Message,
                    null
                );
            }
        }
        #endregion

        #region Airports
        public async Task<(bool Status, int StatusCode, string Message, object Data)> GetAirPortsAsync()
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                var data = (await connection.QueryAsync<object>(
                    "DropDown.Sp_Get_Airports",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                var message = parameters.Get<string>("@Message");

                if (data.Count == 0)
                {
                    return (
                        false,
                        StatusCodes.Status404NotFound,
                        message ?? "No airlines found.",
                        data
                    );
                }

                return (
                    true,
                    StatusCodes.Status200OK,
                    message ?? "Airlines fetched successfully.",
                    data
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    StatusCodes.Status500InternalServerError,
                    ex.Message,
                    new List<object>()
                );
            }
        }
        #endregion

        #region Payment Methods

        public async Task<(int ReturnValue, string Message, IEnumerable<dynamic> Data)> GetPaymentMethods()
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                using var connection = _dapperContext.CreateConnection();
                var result = await connection.QueryAsync(
                    "DropDown.Sp_Get_PaymentMethod",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var message = parameters.Get<string>("@Message") ?? string.Empty;

                return (1, message, result);
            }
            catch (Exception ex)
            {
                return (0, ex.Message, Enumerable.Empty<dynamic>());
            }
        }
        #endregion

        #region Ticket Types

        public async Task<(int ReturnCode, string Message, List<dynamic> Data)> GetTicketTypesDropDownAsync()
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();

            var data = (await connection.QueryAsync(
                "DropDown.Sp_TicketTypes",
                parameters,
                commandType: CommandType.StoredProcedure
            )).ToList();

            int returnCode = parameters.Get<int?>("@ReturnValue") ?? 0;

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            return (
                returnCode,
                message,
                data
            );
        }
        #endregion

        #region Flight Types
        public async Task<(int StatusCode, string Message, IEnumerable<dynamic> Data)> GetFlightTypes()
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();

            var data = await connection.QueryAsync(
                "[DropDown].[Sp_FlightTypes]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (statusCode, message, data);
        }
        #endregion

        #region Flight Route Types

        public async Task<(int StatusCode, string Message, IEnumerable<dynamic> Data)> GetFlightRouteTypes()
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();

            var data = await connection.QueryAsync(
                "[DropDown].[Sp_FlightRouteTypes]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (statusCode, message, data);
        }
        #endregion
    }
}

