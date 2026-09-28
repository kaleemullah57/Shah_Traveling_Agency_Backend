using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.PublicArea.Models;
using Shah_Traveling_Agency_API.Areas.PublicArea.Repositories;
using Shah_Traveling_Agency_API.Areas.TicketHubArea.Models;

namespace Shah_Traveling_Agency_API.Areas.PublicArea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : BaseController
    {

        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly PublicRepo _publicRepo;
        private readonly IHubContext<TicketHub> _ticketHub;

        public PublicController(JwtService jwtService, PasswordService passwordService, PublicRepo PublicRepo, IHubContext<TicketHub> ticketHub)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _publicRepo = PublicRepo;
            _ticketHub = ticketHub;
        }



        #region Pubic Destinations

        [HttpGet("GetPublicDestinations")]
        public async Task<IActionResult> GetPublicDestinations(string? search)
        {
            try
            {
                var result = await _publicRepo.GetPublicDestinations(search);

                if (result.ReturnValue == 1)
                {
                    return Ok(new
                    {
                        statusCode = 200,
                        status = true,
                        message = result.Message,
                        data = result.Data
                    });
                }

                if (result.ReturnValue == 2)
                {
                    return Ok(new
                    {
                        statusCode = 200,
                        status = true,
                        message = result.Message,
                        data = result.Data
                    });
                }

                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = result.Message,
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = ex.Message,
                    data = new List<object>()
                });
            }
        }
        #endregion

        #region Branch Services

        [HttpPost("GetBranchServicesForPublic")]
        public async Task<IActionResult> GetBranchServicesForPublic([FromBody] GetBranchServicesRequest request)
        {
            try
            {
                var result = await _publicRepo.GetBranchServicesForPublicAsync(request);

                if (result.StatusCode == 1)
                {
                    return NotFound(new
                    {
                        status = false,
                        statusCode = 404,
                        message = result.Message,
                        data = result.Data,
                        success = false
                    });
                }

                if (result.StatusCode == -1)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
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
                    data = result.Data,
                    totalCount = result.TotalCount,
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

        #region Shared Tickets
        [HttpPost("GetSharedTickets")]
        public async Task<IActionResult> GetSharedTickets([FromBody] SharedTicketsRequest request)
        {
            try
            {
                var result = await _publicRepo.GetSharedTicketsAsync(request, UserId);

                if (result.ReturnCode == 0)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    });
                }

                if (result.ReturnCode == 2)
                {
                    return Ok(new
                    {
                        status = false,
                        statusCode = 200,
                        message = result.Message,
                        data = new List<SharedTicketModel>(),
                        totalCount = 0,
                        success = true
                    });
                }

                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = result.Message,
                    data = result.Data,
                    totalCount = result.TotalCount,
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

        #region Book Passenger Ticket
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking(
    [FromBody] CreateBookingRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid booking request.",
                        data = (object?)null,
                        success = false
                    });
                }

                if (request.PurchaseInvoiceItemId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Purchase invoice item is required.",
                        data = (object?)null,
                        success = false
                    });
                }

                if (request.Passengers == null ||
                    request.Passengers.Count == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "At least one passenger is required.",
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
                        message = "Unauthorized user.",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _publicRepo.CreateBookingAsync(
                    request,
                    UserId);

                if (result.StatusCode == 200)
                {
                    await _ticketHub.Clients.All.SendAsync(
                        "TicketInventoryUpdated",
                        new
                        {
                            purchaseInvoiceItemId =
                                request.PurchaseInvoiceItemId
                        });
                }

                return StatusCode(
                    result.StatusCode,
                    new
                    {
                        status = result.StatusCode == 200,
                        statusCode = result.StatusCode,
                        message = result.Message,
                        data = result.Data,
                        success = result.StatusCode == 200
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

        #region Customer Bookings

        [HttpPost("GetCustomerBookings")]
        public async Task<IActionResult> GetCustomerBookings(CustomerBookingSearchRequest request)
        {
            try
            {
                if (UserId <= 0)
                {
                    return Unauthorized(new
                    {
                        status = false,
                        statusCode = 401,
                        message = "Unauthorized user.",
                        data = (object?)null,
                        success = false
                    });
                }


                var result = await _publicRepo.GetCustomerBookingsAsync(request, UserId);


                if (result == null || result.Count == 0)
                {
                    return Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = "No bookings found.",
                        data = new List<CustomerBookingModel>(),
                        success = true
                    });
                }


                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Customer bookings retrieved successfully.",
                    data = result,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
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

        #region Cancel Customer Ticket Bookings

        [HttpPost("CancelBookingPassenger")]
        public async Task<IActionResult> CancelBookingPassenger([FromBody] CancelBookingPassengerRequest request)
        {
            try
            {
                if (UserId <= 0)
                {
                    return Unauthorized(new
                    {
                        status = false,
                        statusCode = 401,
                        message = "Unauthorized user.",
                        data = (object?)null,
                        success = false
                    });
                }

                if (request == null || request.BookingPassengerId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Booking passenger is required.",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _publicRepo.CancelBookingPassengerAsync(request, UserId);

                if (result == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Unable to cancel booking passenger.",
                        data = (object?)null,
                        success = false
                    });
                }

                // Notify ticket inventory
                await _ticketHub.Clients.All.SendAsync(
                    "TicketInventoryUpdated",
                    new
                    {
                        purchaseInvoiceItemId =
                            result.PurchaseInvoiceItemId
                    });

                // Notify booking status
                await _ticketHub.Clients.All.SendAsync(
                    "BookingStatusUpdated",
                    new
                    {
                        bookingId = result.BookingId,
                        bookingPassengerId =
                            result.BookingPassengerId
                    });

                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Ticket cancelled successfully.",
                    data = result,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
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
    }
}
