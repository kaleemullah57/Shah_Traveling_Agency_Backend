using Microsoft.Data.SqlClient;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
               _configuration.GetConnectionString("DefaultConnection")
                );
        }
    }
}
