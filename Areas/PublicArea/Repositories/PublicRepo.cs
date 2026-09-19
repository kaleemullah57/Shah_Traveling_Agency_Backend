using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.PublicArea.Models;
using System.Data;
using System.Data.Common;

namespace Shah_Traveling_Agency_API.Areas.PublicArea.Repositories
{
    public class PublicRepo
    {
        private readonly DapperContext _dapperContext;
        public PublicRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        #region Public Destinations

        // Get Public Destinations
        public async Task<(int ReturnValue, string Message, List<GetPublicDestinationModel> Data)> GetPublicDestinations(string? search)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", search, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            parameters.Add("@Message", dbType: System.Data.DbType.String, size: -1, direction: System.Data.ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);

            var data = (await connection.QueryAsync<GetPublicDestinationModel>(
                "Travel.Sp_Get_Destinations",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            )).ToList();

            var returnValue = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message, data);
        }
        #endregion

        #region Branch Services


        public async Task<(int StatusCode, string Message, List<BranchServiceResponse> Data, int TotalCount)> GetBranchServicesForPublicAsync(GetBranchServicesRequest request)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Search", request.Search, DbType.String);

            parameters.Add("@PageNumber", request.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", request.PageSize, DbType.Int32);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();
            var data = (await connection.QueryAsync<BranchServiceResponse>(
                "Travel.Sp_Get_BranchServices_For_Public",
                parameters,
                commandType: CommandType.StoredProcedure
            )).ToList();

            var returnCode = parameters.Get<int>("@ReturnCode");

            var totalCount = parameters.Get<int>("@TotalCount");

            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (
                returnCode,
                message,
                data,
                totalCount
            );
        }
        #endregion

        #region Shared Tickets
        public async Task<(int ReturnCode, string Message, int TotalCount, List<SharedTicketModel> Data)> GetSharedTicketsAsync(SharedTicketsRequest request, int? userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Search", request.Search);
            parameters.Add("@PageNumber", request.PageNumber);
            parameters.Add("@PageSize", request.PageSize);
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);
            parameters.Add("@UserID", userId);
            parameters.Add("@FromSellingPrice", request.FromSellingPrice);
            parameters.Add("@ToSellingPrice", request.ToSellingPrice);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();
            var data = (await connection.QueryAsync<SharedTicketModel>(
                "Data.SP_GetSharedTickets",
                parameters,
                commandType: CommandType.StoredProcedure
            )).ToList();

            int totalCount = parameters.Get<int?>("@TotalCount") ?? 0;

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            int returnCode = parameters.Get<int?>("@ReturnValue") ?? 0;

            return (
                returnCode,
                message,
                totalCount,
                data
            );
        }
        #endregion

    }
}
