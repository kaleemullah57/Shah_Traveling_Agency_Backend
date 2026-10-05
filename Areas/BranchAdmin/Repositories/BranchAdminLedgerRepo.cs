using Dapper;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories
{
    public class BranchAdminLedgerRepo
    {
        private readonly DapperContext _dapperContext;
        public BranchAdminLedgerRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        #region Get Booking

        public async Task<(int ReturnValue, string Message, List<LedgerCustomerBooking> Bookings)> GetLedgerCustomerBookings(GetLedgerCustomerBookingsRequest request, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", request.Search, DbType.String);
            parameters.Add("@CustomerId", request.CustomerId, DbType.Int32);
            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var bookings = (await connection.QueryAsync<LedgerCustomerBooking>(
                "[Data].[SP_Ledger_GetCustomerBookings]",
                parameters,
                commandType: CommandType.StoredProcedure
            )).ToList();

            var returnValue = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message, bookings);
        }


        #endregion

        #region Get Passenger Confirm Tickets

        public async Task<(int ReturnValue, string Message, List<LedgerConfirmedBookingPassenger> Passengers)> GetConfirmedBookingPassengers(GetLedgerConfirmedBookingPassengersRequest request, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", request.CustomerId, DbType.Int32);

            parameters.Add("@BookingId", request.BookingId, DbType.Int32);

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            var passengers =
                (await connection.QueryAsync<LedgerConfirmedBookingPassenger>(
                    "[Data].[SP_Ledger_GetConfirmedBookingPassengers]",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();


            var returnValue = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? string.Empty;


            return (
                returnValue,
                message,
                passengers
            );
        }
        #endregion





    }
}
