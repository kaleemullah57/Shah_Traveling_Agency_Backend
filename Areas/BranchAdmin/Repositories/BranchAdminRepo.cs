using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using System.Data;
using System.Text.Json;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories
{
    public class BranchAdminRepo
    {
        private readonly DapperContext _dapperContext;
        public BranchAdminRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        #region Destinations

        public async Task<(int ReturnValue, string Message)> AddDestination(AddDestinationModel model, List<string>? picturePaths, int createdById, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@DestinationName", model.DestinationName);
            parameters.Add("@Description", model.Description);
            parameters.Add("@CountryId", model.CountryId);
            parameters.Add("@ProvinceId", model.ProvinceId);
            parameters.Add("@CityId", model.CityId);
            parameters.Add("@IsActive", model.IsActive);
            parameters.Add("@CreatedById", createdById);
            parameters.Add("@Branchid", branchId);

            // Multiple paths as JSON
            string? picturePathJson = picturePaths != null &&
                                      picturePaths.Any()
                ? System.Text.Json.JsonSerializer.Serialize(picturePaths)
                : null;

            parameters.Add("@PicturePath", picturePathJson);

            parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Travel.Sp_Add_Destinations",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message);
        }



        // Get Branch Destinations
        public async Task<(int ReturnValue, string Message, List<DestinationModel> Data)> GetDestinationsByBranchId(int branchId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var result = await connection.QueryAsync<DestinationDbModel>(
                "Travel.Sp_Get_Destinations_By_BranchId",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnValue = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? string.Empty;

            var destinations = result.Select(x => new DestinationModel
            {
                DestinationId = x.DestinationId,
                DestinationName = x.DestinationName,
                Description = x.Description,

                CountryId = x.CountryId,
                CountryName = x.CountryName,

                ProvinceId = x.ProvinceId,
                ProvinceName = x.ProvinceName,

                CityId = x.CityId,
                CityName = x.CityName,

                PicturePath = string.IsNullOrWhiteSpace(x.PicturePath)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(x.PicturePath)
                      ?? new List<string>(),

                CreatedById = x.CreatedById,
                CreatedBy = x.CreatedBy,

                CreatedOn = x.CreatedOn,
                IsActive = x.IsActive
            }).ToList();

            return (returnValue, message, destinations);
        }

        private class DestinationDbModel
        {
            public int DestinationId { get; set; }

            public string DestinationName { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;

            public int CountryId { get; set; }

            public string CountryName { get; set; } = string.Empty;

            public int? ProvinceId { get; set; }

            public string? ProvinceName { get; set; }

            public int? CityId { get; set; }

            public string? CityName { get; set; }

            public string? PicturePath { get; set; }

            public int CreatedById { get; set; }

            public string CreatedBy { get; set; } = string.Empty;

            public DateTime CreatedOn { get; set; }

            public bool IsActive { get; set; }
        }







        // Delete Destinations
        public async Task<(int StatusCode, string Message)> DeleteDestination(int destinationId, int branchId, int userId)
        {


            var parameters = new DynamicParameters();

            parameters.Add("@DestinationId", destinationId);
            parameters.Add("@BranchId", branchId);
            parameters.Add("@UserID", userId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteAsync(
                "Travel.Sp_Delete_Destinations_By_BranchAdmin",
                parameters,
                commandType: CommandType.StoredProcedure);

            var message = parameters.Get<string>("@Message") ?? "Unknown error";

            return (result, message);
        }

        #endregion

        #region Branch Services


        // Add Branch Services
        public async Task<(int StatusCode, string Message)> AddBranchService(AddBranchServiceModel model, int userId, int BranchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", BranchId, DbType.Int32);

            parameters.Add("@ServiceId", model.ServiceId, DbType.Int32);

            parameters.Add("@BranchServiceName", model.BranchServiceName, DbType.String, size: 200);

            parameters.Add("@IsActive", model.IsActive, DbType.Boolean);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Travel.Sp_Add_BranchServices",
                parameters,
                commandType: CommandType.StoredProcedure);

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (statusCode, message);
        }





        // Get Branch Services
        public async Task<(int StatusCode, string Message, IEnumerable<BranchServiceModel> Data, int TotalCount)> GetBranchServicesByBranchAdmin(BranchServicesRequest request, int branchId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", request.Search, DbType.String, size: 200);

            parameters.Add("@PageNumber", request.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", request.PageSize, DbType.Int32);

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync<BranchServiceModel>(
                "Travel.Sp_Get_BranchServices_By_BranchAdmin",
                parameters,
                commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? "Unknown response";

            var totalCount = parameters.Get<int?>("@TotalCount") ?? 0;

            return (
                statusCode,
                message,
                data,
                totalCount
            );
        }






        // Delete Branch Services
        public async Task<(int StatusCode, string Message)> DeleteBranchService(int branchServiceId, int branchId, int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchServiceId", branchServiceId, DbType.Int32);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Travel.Sp_Delete_BranchServices",
                parameters,
                commandType: CommandType.StoredProcedure);

            int statusCode = parameters.Get<int>("@ReturnValue");

            string message = parameters.Get<string>("@Message") ?? "Unknown response";

            return (statusCode, message);
        }






        // Update Branch Services
        public async Task<(int StatusCode, string Message)> UpdateBranchService(UpdateBranchServiceRequest request, int userId, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BranchServiceId", request.BranchServiceId, DbType.Int32);

            parameters.Add("@ServiceId", request.ServiceId, DbType.Int32);

            parameters.Add("@IsActive", request.IsActive, DbType.Boolean);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@BranchServiceName", request.BranchServiceName, DbType.String);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Travel.Sp_Update_BranchServices",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnCode = parameters.Get<int>("ReturnValue");

            var message = parameters.Get<string>("@Message");

            return (
                returnCode,
                message ?? string.Empty
            );
        }
        #endregion

        #region Purchase Invoice

        public async Task<(int StatusCode, string Message, int TotalCount, IEnumerable<PurchasedInvoiceModel> Data)> GetPurchasedInvoicesAsync(PurchasedInvoiceSearchRequest request, int userId, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Search", string.IsNullOrWhiteSpace(request.Search) ? null : request.Search);

            parameters.Add("@PageNumber", request.PageNumber <= 0 ? 1 : request.PageNumber);

            parameters.Add("@PageSize", request.PageSize <= 0 ? 20 : request.PageSize);

            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);

            parameters.Add("@UserID", userId);
            parameters.Add("@BranchId", branchId);

            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            var data = (
                await connection.QueryAsync<PurchasedInvoiceModel>(
                    "Inventory.Sp_Get_Purchased_Invoice",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )
            ).ToList();


            // Convert PaymentHistory JSON string into List<>
            foreach (var invoice in data)
            {
                if (!string.IsNullOrWhiteSpace(invoice.PaymentHistoryJson))
                {
                    try
                    {
                        invoice.PaymentHistory =
                            JsonSerializer.Deserialize<
                                List<PurchaseInvoicePaymentHistoryModel>
                            >(invoice.PaymentHistoryJson)
                            ?? new List<PurchaseInvoicePaymentHistoryModel>();
                    }
                    catch
                    {
                        invoice.PaymentHistory =
                            new List<PurchaseInvoicePaymentHistoryModel>();
                    }
                }
                else
                {
                    invoice.PaymentHistory =
                        new List<PurchaseInvoicePaymentHistoryModel>();
                }
            }


            var totalCount = parameters.Get<int?>("@TotalCount") ?? 0;

            var message = parameters.Get<string>("@Message") ?? "";

            var statusCode = parameters.Get<int>("@ReturnValue");


            return (
                statusCode,
                message,
                totalCount,
                data
            );
        }














        // Update Payment Invoice
        public async Task<(bool Status, int StatusCode, string Message, PurchasedInvoicePaymentResponse? Data)> UpdatePaymentInvoice(UpdatePurchasedInvoicePaymentRequest request, int userId, int BranchId)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@PurchaseInvoiceId", request.PurchaseInvoiceId);

                parameters.Add("@BranchId", BranchId);

                parameters.Add("@PaymentAmount", request.PaymentAmount);

                parameters.Add("@PaymentDate", request.PaymentDate);

                parameters.Add("@PaymentMethodId", request.PaymentMethodId);

                parameters.Add("@PaymentReference", request.PaymentReference);

                parameters.Add("@Remarks", request.Remarks);

                parameters.Add("@UserID", userId);

                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                var result = await connection.QueryFirstOrDefaultAsync<PurchasedInvoicePaymentResponse>(
                    "Payment.Sp_Update_PurchasedInvoicePayment",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var message = parameters.Get<string>("@Message");

                if (result == null)
                {
                    return (
                        false,
                        400,
                        message ?? "Unable to add payment.",
                        null
                    );
                }

                return (
                    true,
                    200,
                    message ?? "Payment added successfully.",
                    result
                );
            }
            catch (SqlException ex)
            {
                return (
                    false,
                    500,
                    ex.Message,
                    null
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    500,
                    ex.Message,
                    null
                );
            }
        }

        #endregion

        #region Ticket Inventory 


        // Add Ticket To Inventory
        public async Task<(bool IsSuccess, string Message, int ReturnCode, AddTicketPurchaseResponse? Data)> AddTicketPurchaseAsync(AddTicketPurchaseRequest request, int branchId, int createdById)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@BranchId", branchId, DbType.Int32);
                parameters.Add("@CreatedById", createdById, DbType.Int32);

                parameters.Add("@PurchasedFrom", request.PurchasedFrom);
                parameters.Add("@PurchaseReference", request.PurchaseReference);
                parameters.Add("@InvoiceDate", request.InvoiceDate);

                parameters.Add("@AirlineId", request.AirlineId);
                parameters.Add("@FromAirportId", request.FromAirportId);
                parameters.Add("@ToAirportId", request.ToAirportId);

                parameters.Add("@DepartureDateTime", request.DepartureDateTime);

                parameters.Add("@ArrivalDateTime", request.ArrivalDateTime);

                parameters.Add("@Quantity", request.Quantity);
                parameters.Add("@PurchasePrice", request.PurchasePrice);
                parameters.Add("@SellingPrice", request.SellingPrice);

                parameters.Add("@CheckedBaggageKg", request.CheckedBaggageKg);
                parameters.Add("@HandBaggageKg", request.HandBaggageKg);

                parameters.Add("@PersonalItemKg", request.PersonalItemKg);

                parameters.Add("@ValidFrom", request.ValidFrom);
                parameters.Add("@ValidUntil", request.ValidUntil);

                parameters.Add("@PaidAmount", request.PaidAmount);
                parameters.Add("@PaymentMethodId", request.PaymentMethodId);
                parameters.Add("@PaymentReference", request.PaymentReference);

                parameters.Add("@Remarks", request.Remarks);

                parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

                parameters.Add("@ReturnCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                var data = await connection.QueryFirstOrDefaultAsync<AddTicketPurchaseResponse>(
                    "Inventory.SP_Add_TicketPurchase",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var message = parameters.Get<string>("@Message") ?? string.Empty;

                var returnCode = parameters.Get<int>("@ReturnCode");

                if (returnCode != 0)
                {
                    return (
                        false,
                        message,
                        returnCode,
                        null
                    );
                }

                return (
                    true,
                    message,
                    returnCode,
                    data
                );
            }
            catch (SqlException ex)
            {
                return (
                    false,
                    ex.Message,
                    ex.Number,
                    null
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    ex.Message,
                    -1,
                    null
                );
            }
        }
        #endregion

        #region Update Ticket Selling Price

        public async Task<(int ReturnValue, string Message)> UpdateTicketSellingPriceAsync(UpdateTicketSellingPriceRequest vm, int UserId, int BranchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@PurchaseInvoiceItemId", vm.PurchaseInvoiceItemId, DbType.Int32);

            parameters.Add("@SellingPrice", vm.SellingPrice, DbType.Decimal);

            parameters.Add("@UserId", UserId, DbType.Int32);

            parameters.Add("@BranchId", BranchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "[Inventory].[SP_Update_TicketSellingPrice]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnValue = parameters.Get<int>("@ReturnValue");
            var message = parameters.Get<string>("@Message") ?? string.Empty;

            return (returnValue, message);
        }
        #endregion

        #region Available Tickets

        public async Task<(int ReturnValue, string Message, IEnumerable<AvailableTicketModel> Data)> GetAvailableTicketsAsync(AvailableTicketsRequest vm, int userId, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@Search", vm.Search, DbType.String);

            parameters.Add("@PageNumber", vm.PageNumber, DbType.Int32);

            parameters.Add("@PageSize", vm.PageSize, DbType.Int32);

            parameters.Add("@FromDate", vm.FromDate, DbType.DateTime2);

            parameters.Add("@ToDate", vm.ToDate, DbType.DateTime2);

            parameters.Add("@FromSellingPrice", vm.FromSellingPrice, DbType.Decimal);

            parameters.Add("@ToSellingPrice", vm.ToSellingPrice, DbType.Decimal);

            parameters.Add("@UserID", userId, DbType.Int32);

            parameters.Add("@BranchId", branchId, DbType.Int32);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var data = await connection.QueryAsync<AvailableTicketModel>(
                "[Inventory].[Sp_Get_AvailableTickets]",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnValue = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message")
                          ?? string.Empty;

            return (
                returnValue,
                message,
                data
            );
        }
        #endregion

        #region Share Tickets To Customers
        public async Task<(bool Success, int StatusCode, string Message)> ShareTicketAsync(ShareTicketRequest request, int userId, int branchId)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@PurchaseInvoiceItemId", request.PurchaseInvoiceItemId);

            parameters.Add("@Quantity", request.Quantity);

            parameters.Add("@UserId", userId);

            parameters.Add("@BranchId", branchId);

            parameters.Add("@Message", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Inventory.SP_ShareTicket",
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnCode = parameters.Get<int>("@ReturnValue");

            var message = parameters.Get<string>("@Message") ?? "Unable to share tickets.";

            return (
                returnCode == 6,
                returnCode,
                message
            );
        }
        #endregion
    }
}
