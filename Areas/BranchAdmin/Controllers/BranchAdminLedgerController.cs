using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.TicketHubArea.Models;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchAdminLedgerController : BaseController
    {
        private readonly JwtService _jwtService;
        private readonly BranchAdminLedgerRepo _branchAdminLedgerRepo;
        private readonly IHubContext<TicketHub> _hubcontext;
        private readonly ILogger<BranchAdminLedgerController> _logger;
        public BranchAdminLedgerController(
            JwtService jwtService,
            IHubContext<TicketHub> hubContext,
            BranchAdminLedgerRepo branchAdminLedgerRepo,
            ILogger<BranchAdminLedgerController> logger)
        {
            _jwtService = jwtService;
            _hubcontext = hubContext;
            _branchAdminLedgerRepo = branchAdminLedgerRepo;
            _logger = logger;
        }




        #region Get Customer Bookings


        [HttpPost("GetCustomerBookings")]
        public async Task<IActionResult> GetCustomerBookings([FromBody] GetLedgerCustomerBookingsRequest request)
        {
            try
            {


                if (request.CustomerId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Customer is required.",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _branchAdminLedgerRepo.GetLedgerCustomerBookings(request, BranchId);

                if (result.ReturnValue < 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    });
                }

                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = result.Message,
                    data = result.Bookings,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message = ex.Message,
                    data = (object?)null,
                    success = false
                });
            }
        }
        #endregion

        #region Get Passenger Confirm Tickets

        [HttpPost("GetConfirmedBookingPassengers")]
        public async Task<IActionResult> GetConfirmedBookingPassengers([FromBody] GetLedgerConfirmedBookingPassengersRequest request)
        {
            try
            {

                var result = await _branchAdminLedgerRepo.GetConfirmedBookingPassengers(request, BranchId);


                if (result.ReturnValue < 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    });
                }


                return Ok(new
                {
                    status = "Success",
                    statusCode = 200,
                    message = result.Message,
                    data = result.Passengers,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message = ex.Message,
                    data = (object?)null,
                    success = false
                });
            }
        }
        #endregion

        #region Add Ledger Payment


        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] TicketPaymentAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    statusCode = 400,
                    message = "Please provide valid payment details.",
                    data = (object?)null,
                    success = false
                });
            }

            if (UserId <= 0)
            {
                return Unauthorized(new
                {
                    status = false,
                    statusCode = 401,
                    message = "Unable to identify the authenticated user.",
                    data = (object?)null,
                    success = false
                });
            }

            try
            {
                var result = await _branchAdminLedgerRepo.AddPaymentAsync(model, UserId);

                if (!result.Success || result.Data == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = result.Data,
                        success = false
                    });
                }

                try
                {
                    var paymentEvent = new TicketPaymentAddedEvent
                    {
                        TicketPaymentId = result.Data.TicketPaymentId,
                        CustomerId = model.CustomerId,
                        BookingId = model.BookingId,
                        PaymentMethodId = model.PaymentMethodId,
                        TotalAmount = result.Data.TotalAmount,
                        PaymentDate = model.PaymentDate ?? DateTime.Now,
                        BookingPassengerIds = model.Allocations
                        .Select(x => x.BookingPassengerId)
                        .Distinct()
                        .ToList()
                    };

                    await _hubcontext.Clients
                        .Group(TicketHub.CustomerLedgerGroup(model.CustomerId))
                        .SendAsync("TicketPaymentAdded", paymentEvent);
                }
                catch (Exception ex)
                {
                    // Log ex using your application's logger.
                    // The payment is already committed; don't report it as failed.
                }

                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = result.Message,
                    data = result.Data,
                    success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "AddPayment failed. CustomerId: {CustomerId}, BookingId: {BookingId}, UserId: {UserId}",
                    model.CustomerId,
                    model.BookingId,
                    UserId);

                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message = "An unexpected error occurred while saving the payment.",
                    data = (object?)null,
                    success = false
                });
            }
        }
        #endregion
    }
}
