using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.TicketHubArea.Models;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchAdminController : BaseController
    {

        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly BranchAdminRepo _branchAdminRepo;
        private readonly IHubContext<TicketHub> _hubcontext;

        public BranchAdminController(JwtService jwtService, PasswordService passwordService, BranchAdminRepo branchAdminRepo,IHubContext<TicketHub> hubContext)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _branchAdminRepo = branchAdminRepo;
            _hubcontext = hubContext;
        }






        #region Destinations
        [HttpPost("AddDestination")]
        public async Task<IActionResult> AddDestination([FromForm] AddDestinationModel model)
        {
            try
            {
                var picturePaths = new List<string>();

                if (model.PicturePath != null && model.PicturePath.Any())
                {
                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        "destinations"
                    );

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    foreach (var file in model.PicturePath)
                    {
                        if (file.Length <= 0)
                            continue;

                        string extension = Path.GetExtension(file.FileName);

                        string fileName = $"{Guid.NewGuid()}{extension}";

                        string physicalPath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(
                            physicalPath,
                            FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        string relativePath = $"/uploads/destinations/{fileName}";

                        picturePaths.Add(relativePath);
                    }
                }

                var result = await _branchAdminRepo.AddDestination(model, picturePaths, UserId, BranchId);

                if (result.ReturnValue == 2)
                {
                    return Ok(new
                    {
                        statusCode = 200,
                        status = true,
                        message = result.Message
                    });
                }

                if (result.ReturnValue == 1)
                {
                    return StatusCode(403, new
                    {
                        statusCode = 403,
                        status = false,
                        message = result.Message
                    });
                }

                if (result.ReturnValue == 3)
                {
                    return BadRequest(new
                    {
                        statusCode = 400,
                        status = false,
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = ex.Message
                });
            }
        }


        // Get Branch Destinations
        [HttpGet("GetDestinationsByBranchId")]
        public async Task<IActionResult> GetDestinationsByBranchId()
        {
            try
            {
                var result = await _branchAdminRepo.GetDestinationsByBranchId(BranchId, UserId);

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

                if (result.ReturnValue == 1)
                {
                    return StatusCode(403, new
                    {
                        statusCode = 403,
                        status = false,
                        message = result.Message,
                        data = new List<object>()
                    });
                }

                if (result.ReturnValue == 3)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        status = false,
                        message = result.Message,
                        data = new List<object>()
                    });
                }

                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = result.Message,
                    data = new List<object>()
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





        // Delete Destinations
        [HttpDelete("DeleteDestination/{destinationId}")]
        public async Task<IActionResult> DeleteDestination(int destinationId)
        {
            try
            {
                var result = await _branchAdminRepo.DeleteDestination(destinationId, BranchId, UserId);

                if (result.StatusCode == 1)
                {
                    return Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = result.Message,
                        data = (object?)null,
                        success = true
                    });
                }

                if (result.StatusCode == 2)
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

                return BadRequest(new
                {
                    status = false,
                    statusCode = 400,
                    message = result.Message,
                    data = (object?)null,
                    success = false
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

        #region Branch Services

        // Add Branch Services
        [HttpPost("AddBranchService")]
        public async Task<IActionResult> AddBranchService([FromBody] AddBranchServiceModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid request",
                        data = (object?)null,
                        success = false
                    });
                }


                if (model.ServiceId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid Service ID",
                        data = (object?)null,
                        success = false
                    });
                }

                if (string.IsNullOrWhiteSpace(model.BranchServiceName))
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Branch Service Name is required",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _branchAdminRepo.AddBranchService(model, UserId, BranchId);

                return result.StatusCode switch
                {
                    3 => Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = result.Message,
                        data = new
                        {
                            branchId = BranchId,
                            serviceId = model.ServiceId,
                            branchServiceName = model.BranchServiceName,
                            isActive = model.IsActive
                        },
                        success = true
                    }),

                    2 => Conflict(new
                    {
                        status = false,
                        statusCode = 409,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    }),

                    1 => StatusCode(403, new
                    {
                        status = false,
                        statusCode = 403,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    }),

                    4 => BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    }),

                    _ => StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message,
                        data = (object?)null,
                        success = false
                    })
                };
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






        // Get Branch Services
        [HttpPost("GetBranchServices")]
        public async Task<IActionResult> GetBranchServices(BranchServicesRequest request)
        {
            try
            {
                var result = await _branchAdminRepo.GetBranchServicesByBranchAdmin(request, BranchId, UserId);

                if (result.StatusCode == 1)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            status = false,
                            statusCode = 403,
                            message = result.Message,
                            data = Array.Empty<BranchServiceModel>(),
                            totalCount = 0,
                            success = false
                        });
                }

                if (result.StatusCode == 3)
                {
                    return NotFound(
                        new
                        {
                            status = false,
                            statusCode = 404,
                            message = result.Message,
                            data = Array.Empty<BranchServiceModel>(),
                            totalCount = 0,
                            success = false
                        });
                }

                if (result.StatusCode == 2)
                {
                    return Ok(
                        new
                        {
                            status = true,
                            statusCode = 200,
                            message = result.Message,
                            data = result.Data,
                            totalCount = result.TotalCount,
                            success = true
                        });
                }

                return BadRequest(
                    new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = Array.Empty<BranchServiceModel>(),
                        totalCount = 0,
                        success = false
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        status = false,
                        statusCode = 500,
                        message = ex.Message,
                        data = Array.Empty<BranchServiceModel>(),
                        totalCount = 0,
                        success = false
                    });
            }
        }





        // Delete Branch Services
        [HttpDelete("DeleteBranchService/{branchServiceId}")]
        public async Task<IActionResult> DeleteBranchService(int branchServiceId)
        {
            try
            {

                var result = await _branchAdminRepo.DeleteBranchService(branchServiceId, BranchId, UserId);
                if (result.StatusCode == 1)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            status = false,
                            statusCode = 403,
                            message = result.Message,
                            data = Array.Empty<object>(),
                            success = false
                        });
                }
                if (result.StatusCode == 2)
                {
                    return Ok(
                        new
                        {
                            status = true,
                            statusCode = 200,
                            message = result.Message,
                            data = Array.Empty<object>(),
                            success = true
                        });
                }
                if (result.StatusCode == 3)
                {
                    return NotFound(
                        new
                        {
                            status = false,
                            statusCode = 404,
                            message = result.Message,
                            data = Array.Empty<object>(),
                            success = false
                        });
                }
                return BadRequest(
                    new
                    {
                        status = false,
                        statusCode = 400,
                        message = result.Message,
                        data = Array.Empty<object>(),
                        success = false
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        status = false,
                        statusCode = 500,
                        message = ex.Message,
                        data = Array.Empty<object>(),
                        success = false
                    });
            }
        }







        // Update Branch Services
        [HttpPut("UpdateBranchService")]
        public async Task<IActionResult> UpdateBranchService([FromBody] UpdateBranchServiceRequest request)
        {
            try
            {
                if (UserId <= 0)
                {
                    return Unauthorized(new
                    {
                        status = false,
                        statusCode = 401,
                        message = "Invalid User",
                        data = (object?)null,
                        success = false
                    });
                }

                if (BranchId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid Branch",
                        data = (object?)null,
                        success = false
                    });
                }

                var result = await _branchAdminRepo.UpdateBranchService(request, UserId, BranchId);

                switch (result.StatusCode)
                {
                    case 4:
                        return Ok(new
                        {
                            status = true,
                            statusCode = 200,
                            message = result.Message,
                            data = (object?)null,
                            success = true
                        });

                    case 1:
                        return StatusCode(403, new
                        {
                            status = false,
                            statusCode = 403,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });

                    case 2:
                        return NotFound(new
                        {
                            status = false,
                            statusCode = 404,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });

                    case 3:
                        return Conflict(new
                        {
                            status = false,
                            statusCode = 409,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });

                    case 5:
                        return BadRequest(new
                        {
                            status = false,
                            statusCode = 400,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });

                    default:
                        return StatusCode(500, new
                        {
                            status = false,
                            statusCode = 500,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });
                }
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

        #region Purchase Invoice


        [HttpPost("GetPurchasedInvoices")]
        public async Task<IActionResult> GetPurchasedInvoices([FromBody] PurchasedInvoiceSearchRequest request)
        {
            try
            {


                var result = await _branchAdminRepo.GetPurchasedInvoicesAsync(request, UserId, BranchId);


                if (result.StatusCode == 2)
                {
                    return Ok(new
                    {
                        status = false,
                        statusCode = 404,
                        message = result.Message,
                        data = result.Data,
                        totalCount = result.TotalCount,
                        success = false
                    });
                }


                if (result.StatusCode == 0)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message,
                        data = result.Data,
                        totalCount = result.TotalCount,
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
                    totalCount = 0,
                    success = false
                });
            }
        }










        // Update Payment Invoice
        [HttpPost("UpdatePurchasedInvoicePayment")]
        public async Task<IActionResult> UpdatePurchasedInvoicePayment(UpdatePurchasedInvoicePaymentRequest request)
        {
            try
            {

                var result = await _branchAdminRepo.UpdatePaymentInvoice(request, UserId, BranchId);

                return StatusCode(result.StatusCode, new
                {
                    status = result.Status,
                    statusCode = result.StatusCode,
                    message = result.Message,
                    data = result.Data,
                    success = result.Status
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

        #region Ticket Inventory

        [HttpPost("AddTicketPurchase")]
        public async Task<IActionResult> AddTicketPurchase([FromBody] AddTicketPurchaseRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Request is required.",
                        data = (object?)null,
                        success = false
                    });
                }



                var result = await _branchAdminRepo.AddTicketPurchaseAsync(request, BranchId, UserId);


                if (!result.IsSuccess)
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
                    data = result.Data,
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

        #region Update Ticket Selling Price

        [HttpPut("UpdateTicketSellingPrice")]
        public async Task<IActionResult> UpdateTicketSellingPrice([FromBody] UpdateTicketSellingPriceRequest request)
        {
            try
            {
                if (request.PurchaseInvoiceItemId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid PurchaseInvoiceItemId.",
                        data = (object?)null,
                        success = false
                    });
                }

                if (request.SellingPrice <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Selling price must be greater than zero.",
                        data = (object?)null,
                        success = false
                    });
                }
                var (returnValue, message) = await _branchAdminRepo.UpdateTicketSellingPriceAsync(request, UserId, BranchId);

                switch (returnValue)
                {
                    case 3:
                        await _hubcontext.Clients.All.SendAsync(
                            "Ticket Price Updated", new
                            {
                                purchaseinvoiceid = request.PurchaseInvoiceItemId,
                                sellingPrice = request.SellingPrice
                            }
                            );
                        return Ok(new
                        {
                            status = true,
                            statusCode = 200,
                            message,
                            data = (object?)null,
                            success = true
                        });

                    case 1:
                        return StatusCode(403, new
                        {
                            status = false,
                            statusCode = 403,
                            message,
                            data = (object?)null,
                            success = false
                        });

                    case 2:
                        return NotFound(new
                        {
                            status = false,
                            statusCode = 404,
                            message,
                            data = (object?)null,
                            success = false
                        });

                    case 4:
                        return BadRequest(new
                        {
                            status = false,
                            statusCode = 400,
                            message,
                            data = (object?)null,
                            success = false
                        });

                    default:
                        return StatusCode(500, new
                        {
                            status = false,
                            statusCode = 500,
                            message,
                            data = (object?)null,
                            success = false
                        });
                }
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

        #region Available Tickets

        [HttpPost("GetAvailableTickets")]
        public async Task<IActionResult> GetAvailableTickets([FromBody] AvailableTicketsRequest request)
        {
            try
            {
                var result = await _branchAdminRepo.GetAvailableTicketsAsync(request, UserId, BranchId);

                switch (result.ReturnValue)
                {
                    case 2:

                        return Ok(new
                        {
                            status = true,
                            statusCode = 200,
                            message = result.Message,
                            data = result.Data,
                            success = true
                        });

                    case 1:

                        return StatusCode(403, new
                        {
                            status = false,
                            statusCode = 403,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });

                    case 3:

                        return Ok(new
                        {
                            status = false,
                            statusCode = 200,
                            message = result.Message,
                            data = new List<AvailableTicketModel>(),
                            success = false
                        });

                    default:

                        return StatusCode(500, new
                        {
                            status = false,
                            statusCode = 500,
                            message = result.Message,
                            data = (object?)null,
                            success = false
                        });
                }
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

        #region Share Tickets To Customers

        [HttpPost("ShareTicketsToCustomers")]
        public async Task<IActionResult> ShareTicket([FromBody] ShareTicketRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "Invalid request.",
                        data = (object?)null,
                        success = false
                    });
                }


                var result = await _branchAdminRepo.ShareTicketAsync(request, UserId,BranchId);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = result.StatusCode,
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
                    data = (object?)null,
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
