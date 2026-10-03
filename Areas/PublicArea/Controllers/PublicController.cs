using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
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
        private readonly BranchAdminRepo _branchAdminRepo;
        private readonly IHubContext<TicketHub> _ticketHub;

        public PublicController(JwtService jwtService, PasswordService passwordService, PublicRepo PublicRepo, IHubContext<TicketHub> ticketHub, BranchAdminRepo branchAdminRepo)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _publicRepo = PublicRepo;
            _ticketHub = ticketHub;
            _branchAdminRepo = branchAdminRepo;
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
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
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
                    // ============================================================
                    // INVENTORY UPDATED
                    // ============================================================

                    await _ticketHub.Clients.All.SendAsync(
                        "TicketInventoryUpdated",
                        new
                        {
                            purchaseInvoiceItemId =
                                request.PurchaseInvoiceItemId
                        });

                    // ============================================================
                    // NEW BOOKING NOTIFICATION
                    // ============================================================

                    try
                    {
                        var notification =
                            await _branchAdminRepo.CreateCustomerNotification(
                                UserId,
                                "NewBooking",
                                "New Booking Received",
                                "A new ticket booking has been created.",
                                result.Data?.BookingId,
                                result.Data?.BookingPassengerId
                            );

                        if (notification.ReturnValue != 0)
                        {
                            Console.WriteLine(
                                $"New booking notification failed: {notification.Message}"
                            );
                        }
                        else
                        {
                            // Tell connected clients to refresh notifications.
                            // The actual notification is retrieved through
                            // GET /api/Public/MyNotifications.
                            await _ticketHub.Clients.All.SendAsync(
                                "NotificationUpdated"
                            );
                        }
                    }
                    catch (Exception notificationEx)
                    {
                        Console.WriteLine(
                            $"New booking notification error: " +
                            notificationEx.Message
                        );
                    }
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
        public async Task<IActionResult> CancelBookingPassenger(CancelBookingPassengerRequest request)
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

                var result =
                    await _publicRepo.CancelBookingPassengerAsync(
                        request,
                        UserId);

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



                // =========================================================
                // 1. SAVE CUSTOMER NOTIFICATION
                // =========================================================

                // =========================================================
                // 1. SAVE CUSTOMER NOTIFICATION + REALTIME NOTIFICATION
                // =========================================================

                try
                {
                    // Customer who owns this booking
                    var customerId = result.CustomerId;

                    var notification =
                        await _branchAdminRepo.CreateCustomerNotification(
                            customerId,
                            "TicketCancelled",
                            result.CancellationTypeName ?? "Ticket Cancelled",
                            request.CancellationReason ??
                                "Your ticket has been cancelled by Branch Admin.",
                            result.BookingId,
                            result.BookingPassengerId
                        );

                    if (
                        notification.ReturnValue == 0 &&
                        notification.Data != null
                    )
                    {
                        
                        await _ticketHub.Clients
                            .User(customerId.ToString())
                            .SendAsync(
                                "CustomerNotification",
                                new
                                {
                                    notificationId =
                                        notification.Data.NotificationId,

                                    customerId =
                                        notification.Data.CustomerId,

                                    notificationType =
                                        notification.Data.NotificationType,

                                    title =
                                        notification.Data.Title,

                                    message =
                                        notification.Data.Message,

                                    bookingId =
                                        notification.Data.BookingId,

                                    bookingPassengerId =
                                        notification.Data.BookingPassengerId,

                                    isRead =
                                        notification.Data.IsRead,

                                    createdDate =
                                        notification.Data.CreatedDate,

                                    readDate =
                                        notification.Data.ReadDate
                                });
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Notification creation failed: " +
                            $"{notification.Message}"
                        );
                    }
                }
                catch (Exception notificationEx)
                {
                    Console.WriteLine(
                        $"Customer notification error: " +
                        $"{notificationEx.Message}"
                    );
                }


                // =========================================================
                // 3. INVENTORY UPDATED
                // =========================================================

                await _ticketHub.Clients.All.SendAsync(
                    "TicketInventoryUpdated",
                    new
                    {
                        purchaseInvoiceItemId =
                            result.PurchaseInvoiceItemId
                    });


                // =========================================================
                // 4. BOOKING STATUS UPDATED
                // =========================================================

                await _ticketHub.Clients.All.SendAsync(
                    "BookingStatusUpdated",
                    new
                    {
                        bookingId =
                            result.BookingId,

                        bookingPassengerId =
                            result.BookingPassengerId
                    });


                // =========================================================
                // 5. BOOKING PASSENGER CANCELLED
                // =========================================================

                await _ticketHub.Clients.All.SendAsync(
                    "BookingPassengerCancelled",
                    new
                    {
                        bookingId =
                            result.BookingId,

                        bookingPassengerId =
                            result.BookingPassengerId,

                        purchaseInvoiceItemId =
                            result.PurchaseInvoiceItemId,

                        bookingStatusId =
                            result.BookingStatusId,

                        bookingStatus =
                            result.BookingStatus,

                        cancellationTypeId =
                            result.CancellationTypeId,

                        cancellationTypeName =
                            result.CancellationTypeName
                    });


                // =========================================================
                // 6. RESPONSE
                // =========================================================

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

        #region Get Notifications

        [HttpGet("MyNotifications")]
        public async Task<IActionResult> MyNotifications()
        {
            try
            {

                var data = await _publicRepo.GetCustomerNotifications(UserId,UserTypeId, BranchId);

                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Notifications retrieved successfully.",
                    data = data,
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







        // Mark Notifications
        [HttpPost("MarkNotificationRead")]
        public async Task<IActionResult> MarkNotificationRead([FromBody] MarkNotificationReadRequest request)
        {
            try
            {
                if (request == null || request.NotificationId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Notification is required.",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _publicRepo.ReadCustomerNotification(request.NotificationId);

                if (result.ReturnValue != 0)
                {
                    return NotFound(new
                    {
                        status = false,
                        statusCode = 404,
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
                    data = new
                    {
                        notificationId = request.NotificationId
                    },
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
    }
}
