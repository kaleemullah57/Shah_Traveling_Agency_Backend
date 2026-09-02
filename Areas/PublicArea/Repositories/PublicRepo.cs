using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.PublicArea.Models;

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
        public async Task<(int ReturnValue, string Message, List<GetPublicDestinationModel> Data)>
    GetPublicDestinations(string? search)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@Search",
                search,
                System.Data.DbType.String,
                System.Data.ParameterDirection.Input
            );

            parameters.Add(
                "@Message",
                dbType: System.Data.DbType.String,
                size: -1,
                direction: System.Data.ParameterDirection.Output
            );

            parameters.Add(
                "@ReturnValue",
                dbType: System.Data.DbType.Int32,
                direction: System.Data.ParameterDirection.ReturnValue
            );

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

    }
}
