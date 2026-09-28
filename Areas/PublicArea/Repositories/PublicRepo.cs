using Dapper;
using Microsoft.Data.SqlClient;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.PublicArea.Models;
using System.Data;
using System.Data.Common;
using System.Text.Json;

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
            parameters.Add("@FromDate", request.FromDate, dbType: DbType.Date);
            parameters.Add("@ToDate", request.ToDate, dbType: DbType.Date);
            parameters.Add("@UserID", userId);
            parameters.Add("@FromSellingPrice", request.FromSellingPrice);
            parameters.Add("@ToSellingPrice", request.ToSellingPrice);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using var connection = _dapperContext.CreateConnection();
            var dbData = (
        await connection.QueryAsync<SharedTicketModel>(
            "Data.SP_GetSharedTickets",
            parameters,
            commandType: CommandType.StoredProcedure
        )
    ).ToList();

            var data = dbData.Select(x =>
            {
                var ticket = new SharedTicketModel
                {
                    PurchaseInvoiceItemId = x.PurchaseInvoiceItemId,
                    AirlineName = x.AirlineName,
                    AirlineCode = x.AirlineCode,
                    FromAirport = x.FromAirport,
                    ToAirport = x.ToAirport,
                    FromCountry = x.FromCountry,
                    ToCountry = x.ToCountry,
                    DepartureDateTime = x.DepartureDateTime,
                    ArrivalDateTime = x.ArrivalDateTime,
                    AvailableQuantity = x.AvailableQuantity,
                    SellingPrice = x.SellingPrice,
                    CheckedBaggageKg = x.CheckedBaggageKg,
                    HandBaggageKg = x.HandBaggageKg,
                    PersonalItemKg = x.PersonalItemKg,
                    ValidFrom = x.ValidFrom,
                    ValidUntil = x.ValidUntil,
                    BranchId = x.BranchId,
                    BranchName = x.BranchName,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    TicketTypeId = x.TicketTypeId,
                    TicketTypeName = x.TicketTypeName,

                    Stops = string.IsNullOrWhiteSpace(x.StopsJson)
                        ? new List<SharedTicketStopModel>()
                        : JsonSerializer.Deserialize<List<SharedTicketStopModel>>(
                            x.StopsJson
                        ) ?? new List<SharedTicketStopModel>()
                };

                return ticket;
            }).ToList();

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

        #region Book Passenger Ticket
        public async Task<(int StatusCode, string Message, CreateBookingResponse? Data)> CreateBookingAsync(CreateBookingRequest request, int customerId)
        {
            using var connection = _dapperContext.CreateConnection();


            var passengerTable = new DataTable();

            passengerTable.Columns.Add("PassengerTypeId", typeof(int));
            passengerTable.Columns.Add("FullName", typeof(string));
            passengerTable.Columns.Add("PassportNumber", typeof(string));
            passengerTable.Columns.Add("DateOfBirth", typeof(DateTime));
            passengerTable.Columns.Add("Gender", typeof(string));
            passengerTable.Columns.Add("Nationality", typeof(string));
            passengerTable.Columns.Add("ContactNumber", typeof(string));
            passengerTable.Columns.Add("Email", typeof(string));

            foreach (var passenger in request.Passengers)
            {
                var row = passengerTable.NewRow();

                row["PassengerTypeId"] = passenger.PassengerTypeId;
                row["FullName"] = passenger.FullName;

                row["PassportNumber"] =
                    string.IsNullOrWhiteSpace(passenger.PassportNumber)
                        ? DBNull.Value
                        : passenger.PassportNumber;

                row["DateOfBirth"] =
                    passenger.DateOfBirth.HasValue
                        ? passenger.DateOfBirth.Value
                        : DBNull.Value;

                row["Gender"] =
                    string.IsNullOrWhiteSpace(passenger.Gender)
                        ? DBNull.Value
                        : passenger.Gender;

                row["Nationality"] =
                    string.IsNullOrWhiteSpace(passenger.Nationality)
                        ? DBNull.Value
                        : passenger.Nationality;

                row["ContactNumber"] =
                    string.IsNullOrWhiteSpace(passenger.ContactNumber)
                        ? DBNull.Value
                        : passenger.ContactNumber;

                row["Email"] =
                    string.IsNullOrWhiteSpace(passenger.Email)
                        ? DBNull.Value
                        : passenger.Email;

                passengerTable.Rows.Add(row);
            }

            var parameters = new DynamicParameters();

            parameters.Add("@PurchaseInvoiceItemId", request.PurchaseInvoiceItemId, DbType.Int32);

            parameters.Add("@CustomerId", customerId, DbType.Int32);

            parameters.Add("@Passengers", passengerTable.AsTableValuedParameter("Booking.TableType_BookingPassenger"));

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryFirstOrDefaultAsync<CreateBookingResponse>(
                "Booking.SP_CreateBooking",
                parameters,
                commandType: CommandType.StoredProcedure);

            var message = parameters.Get<string>("@Message") ?? "Booking request processed.";

            var statusCode = parameters.Get<int>("@ReturnValue");

            return (statusCode, message, data);
        }
        #endregion

        #region Customer Bookings

        public async Task<List<CustomerBookingModel>> GetCustomerBookingsAsync(CustomerBookingSearchRequest request, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", request.Search, DbType.String);

            parameters.Add("@PageNumber", request.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", request.PageSize, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);


            var result = await connection.QueryAsync<CustomerBookingDbModel>(
                    "[Booking].[Sp_Get_CustomerBooking]",
                    parameters,
                    commandType: CommandType.StoredProcedure);


            var bookings = result.ToList();

            var response = new List<CustomerBookingModel>();


            foreach (var item in bookings)
            {
                var booking = new CustomerBookingModel
                {
                    BookingId = item.BookingId,

                    BookingReference = item.BookingReference,

                    CustomerId = item.CustomerId,

                    CreatedBy = item.CreatedBy,

                    CreatedDate = item.CreatedDate,

                    PurchaseInvoiceItemId = item.PurchaseInvoiceItemId,

                    BookedTickets = item.BookedTickets,

                    BookingStatusId = item.BookingStatusId,

                    BookingStatus = item.BookingStatus,

                    PassengerBookingDetails = new List<CustomerBookingPassengerModel>()
                };


                if (!string.IsNullOrWhiteSpace(
                    item.PassengerBookingDetails))
                {
                    booking.PassengerBookingDetails =
                        JsonSerializer.Deserialize<
                            List<CustomerBookingPassengerModel>>(
                                item.PassengerBookingDetails)
                        ?? new List<CustomerBookingPassengerModel>();
                }


                response.Add(booking);
            }


            return response;
        }
        #endregion

    }
}
