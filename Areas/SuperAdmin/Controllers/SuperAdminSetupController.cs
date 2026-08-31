using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Models;
using Shah_Traveling_Agency_API.Areas.SuperAdmin.Repositories;

namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuperAdminSetupController : BaseController
    {
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly SuperAdminSetupRepo _superAdminSetupRepo;

        public SuperAdminSetupController(JwtService jwtService, PasswordService passwordService, SuperAdminSetupRepo superAdminSetupRepo)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _superAdminSetupRepo = superAdminSetupRepo;
        }




        #region Branches


        // Get Branches
        [HttpPost("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches(BranchVM vm)
        {
            try
            {


                var result =
                    await _superAdminSetupRepo.GetAllBranchesAsync(vm, UserId);

                switch (result.StatusCode)
                {

                    case 0:

                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                success = false,
                                statusCode =
                                    StatusCodes.Status500InternalServerError,
                                message = result.Message
                            }
                        );


                    case 1:

                        return StatusCode(
                            StatusCodes.Status403Forbidden,
                            new
                            {
                                success = false,
                                statusCode =
                                    StatusCodes.Status403Forbidden,
                                message = result.Message
                            }
                        );

                    case 2:

                        return Ok(new
                        {
                            success = true,
                            statusCode = StatusCodes.Status200OK,
                            message = result.Message,
                            data = result.Data
                        });

                    case 3:

                        return NotFound(new
                        {
                            success = false,
                            statusCode = StatusCodes.Status404NotFound,
                            message = result.Message,
                            data = new List<GetBranchModel>()
                        });

                    default:

                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                success = false,
                                statusCode =
                                    StatusCodes.Status500InternalServerError,
                                message =
                                    "Unexpected response from the server.",
                                data = new List<GetBranchModel>()
                            }
                        );
                }
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        statusCode =
                            StatusCodes.Status500InternalServerError,
                        message =
                            $"An unexpected API error occurred: {ex.Message}"
                    }
                );
            }
        }




        // Save Branches
        [HttpPost("AddBranch")]
        public async Task<IActionResult> AddBranch(AddBranchModel model)
        {
            Console.WriteLine($"BranchName: {model.BranchName}");
            Console.WriteLine($"Location: {model.Location}");
            Console.WriteLine($"IsActive: {model.IsActive}");
            Console.WriteLine($"UserId: {UserId}");
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Invalid request."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.BranchName))
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Branch name is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.Location))
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = StatusCodes.Status400BadRequest,
                        message = "Branch location is required."
                    });
                }

                var result = await _superAdminSetupRepo.AddBranchAsync(model, UserId);

                switch (result.StatusCode)
                {
                    case 3:

                        return StatusCode(
                            StatusCodes.Status201Created,
                            new
                            {
                                success = true,
                                statusCode = StatusCodes.Status201Created,
                                message = result.Message
                            }
                        );
                    case 1:

                        return StatusCode(
                            StatusCodes.Status403Forbidden,
                            new
                            {
                                success = false,
                                statusCode = StatusCodes.Status403Forbidden,
                                message = result.Message
                            }
                        );
                    case 2:

                        return Conflict(
                            new
                            {
                                success = false,
                                statusCode = StatusCodes.Status409Conflict,
                                message = result.Message
                            }
                        );
                    case 4:

                        return BadRequest(
                            new
                            {
                                success = false,
                                statusCode = StatusCodes.Status400BadRequest,
                                message = result.Message
                            }
                        );

                    // ==========================================
                    // DATABASE / API ERROR
                    // ==========================================
                    case 0:

                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                success = false,
                                statusCode =
                                    StatusCodes.Status500InternalServerError,
                                message = result.Message
                            }
                        );
                    default:

                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                success = false,
                                statusCode =
                                    StatusCodes.Status500InternalServerError,
                                message = "Unexpected response from the server."
                            }
                        );
                }
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        statusCode =
                            StatusCodes.Status500InternalServerError,
                        message =
                            $"An unexpected API error occurred: {ex.Message}"
                    }
                );
            }
        }

        #endregion

        #region Countries

        // Get Countries
        [HttpPost("GetCountries")]
        public async Task<IActionResult> GetCountries(CountryVM vm)
        {
            try
            {
                var result = await _superAdminSetupRepo.GetCountriesAsync(vm, UserId);

                return result.ReturnValue switch
                {
                    0 => StatusCode(500, new
                    {
                        success = false,
                        statusCode = 500,
                        message = result.Message,
                        data = result.Data
                    }),

                    1 => StatusCode(403, new
                    {
                        success = false,
                        statusCode = 403,
                        message = result.Message,
                        data = result.Data
                    }),

                    2 => Ok(new
                    {
                        success = true,
                        statusCode = 200,
                        message = result.Message,
                        data = result.Data
                    }),

                    3 => Ok(new
                    {
                        success = true,
                        statusCode = 200,
                        message = result.Message,
                        data = result.Data
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        statusCode = 500,
                        message = "Unknown response from stored procedure.",
                        data = result.Data
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = ex.Message,
                    data = Array.Empty<object>()
                });
            }
        }



        // Add Countries
        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry(AddCountryRequest model)
        {
            try
            {

                var result = await _superAdminSetupRepo.AddCountryAsync(model, UserId);

                return result.ReturnValue switch
                {
                    0 => StatusCode(500, new
                    {
                        success = false,
                        statusCode = 500,
                        message = result.Message
                    }),

                    1 => StatusCode(403, new
                    {
                        success = false,
                        statusCode = 403,
                        message = result.Message
                    }),

                    2 => Conflict(new
                    {
                        success = false,
                        statusCode = 409,
                        message = result.Message
                    }),

                    3 => Ok(new
                    {
                        success = true,
                        statusCode = 200,
                        message = result.Message
                    }),

                    4 => BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = result.Message
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        statusCode = 500,
                        message = "Unknown response from stored procedure."
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Provinces

        // Add Provinces
        [HttpPost("AddProvince")]
        public async Task<IActionResult> AddProvince(AddProvinceModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Province data is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.ProvinceName))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Province name is required"
                    });
                }

                var (success, statusCode, message) = await _superAdminSetupRepo.AddProvince(model, UserId);

                return StatusCode(statusCode, new
                {
                    success,
                    statusCode,
                    message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        #endregion

    }
}
