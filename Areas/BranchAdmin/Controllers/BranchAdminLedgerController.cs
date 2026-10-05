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

        public BranchAdminLedgerController(JwtService jwtService, IHubContext<TicketHub> hubContext, BranchAdminLedgerRepo branchAdminLedgerRepo)
        {
            _jwtService = jwtService;
            _hubcontext = hubContext;
            _branchAdminLedgerRepo = branchAdminLedgerRepo;
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
    }
}
