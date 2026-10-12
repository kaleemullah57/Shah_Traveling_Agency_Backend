using Dapper;
using Microsoft.Data.SqlClient;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using System.Data;
using System.Text.Json;

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

        #region Add Ledger Payment

        public async Task<(bool Success, string Message, TicketPaymentAddResponse? Data)> AddPaymentAsync(TicketPaymentAddModel model, int createdBy)
        {

            using var connection = _dapperContext.CreateConnection();

            var allocationsJson = JsonSerializer.Serialize(
                model.Allocations,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            var parameters = new DynamicParameters();

            parameters.Add("@CustomerId", model.CustomerId, DbType.Int32);
            parameters.Add("@BookingId", model.BookingId, DbType.Int32);
            parameters.Add("@PaymentMethodId", model.PaymentMethodId, DbType.Int32);
            parameters.Add("@TotalAmount", model.TotalAmount, DbType.Decimal);
            parameters.Add("@PaymentDate", model.PaymentDate, DbType.DateTime2);
            parameters.Add("@PaymentReference", model.PaymentReference, DbType.String, size: 100);
            parameters.Add("@Remarks", model.Remarks, DbType.String, size: 500);
            parameters.Add("@AllocationsJson", allocationsJson, DbType.String);
            parameters.Add("@CreatedBy", createdBy, DbType.Int32);

            parameters.Add(
                "@Message",
                dbType: DbType.String,
                direction: ParameterDirection.Output,
                size: 500);

            parameters.Add(
                "@ReturnCode",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            var command = new CommandDefinition(
                commandText: "Ledger.SP_TicketPayment_Add",
                parameters: parameters,
                commandType: CommandType.StoredProcedure);

            var rows = (await connection
                .QueryAsync<TicketPaymentAddResponse>(command))
                .ToList();

            var returnCode = parameters.Get<int>("@ReturnCode");

            var message = parameters.Get<string>("@Message")
                          ?? "Unable to process payment.";

            if (returnCode != 0)
                return (false, message, null);

            var data = rows.FirstOrDefault();

            if (data == null)
                return (false, "Payment response was not returned.", null);

            return (true, message, data);

        }

        #endregion



    }
}
