using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
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

    }
}
