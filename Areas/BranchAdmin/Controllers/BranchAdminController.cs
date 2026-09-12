using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Models;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Repositories;

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

        public BranchAdminController(JwtService jwtService, PasswordService passwordService, BranchAdminRepo branchAdminRepo)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _branchAdminRepo = branchAdminRepo;
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
        #endregion


    }
}
