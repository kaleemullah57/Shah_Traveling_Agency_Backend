using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.DropDown.Repositories;
using Shah_Traveling_Agency_API.Areas.PublicArea.Repositories;
using System.Data;

namespace Shah_Traveling_Agency_API.Areas.DropDown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DropDownController : BaseController
    {
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly DropDownRepo _dropDownRepo;

        public DropDownController(JwtService jwtService, PasswordService passwordService, DropDownRepo dropDownRepo)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _dropDownRepo = dropDownRepo;
        }




        #region Countries
        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _dropDownRepo.GetCountries();
                return Ok(new
                {
                    statusCode = 200,
                    status = true,
                    message = "Countries retrieved successfully.",
                    data = countries
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = false,
                    message = ex.Message,
                    data = (object?)null
                });
            }
        }
        #endregion

        #region Provinces

        [HttpGet("GetProvincesByCountryId")]
        public async Task<IActionResult> GetProvincesByCountryId(int countryId)
        {
            try
            {
                var result = await _dropDownRepo.GetProvincesByCountryIdAsync(countryId);

                if (result.ReturnValue == 0)
                {
                    return StatusCode(500, new
                    {
                        statusCode = 500,
                        status = "Error",
                        message = result.Message ?? "Unable to retrieve provinces.",
                        data = Array.Empty<object>()
                    });
                }

                if (result.ReturnValue == 2)
                {
                    return Ok(new
                    {
                        statusCode = 200,
                        status = "Success",
                        message = result.Message ?? "No provinces found.",
                        data = result.Data
                    });
                }

                return Ok(new
                {
                    statusCode = 200,
                    status = "Success",
                    message = result.Message ?? "Provinces retrieved successfully.",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    statusCode = 500,
                    status = "Exception Error",
                    message = ex.Message,
                    data = Array.Empty<object>()
                });
            }
        }
        #endregion

        #region Cities DropDown
        [HttpGet("GetCitiesByProvinceId")]
        public async Task<IActionResult> GetCitiesByProvinceId([FromQuery] int provinceId)
        {
            try
            {
                if (provinceId <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        statusCode = 400,
                        message = "A valid ProvinceId is required",
                        data = new List<object>(),
                        success = false
                    });
                }

                var result = await _dropDownRepo.GetCitiesByProvinceIdAsync(provinceId);

                if (result.ReturnValue == 0)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message ?? "An error occurred while fetching cities",
                        data = new List<object>(),
                        success = false
                    });
                }
                if (result.ReturnValue == 1)
                {
                    return Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = result.Message ?? "Cities fetched successfully",
                        data = result.Data,
                        success = true
                    });
                }

                if (result.ReturnValue == 2)
                {
                    return NotFound(new
                    {
                        status = false,
                        statusCode = 404,
                        message = result.Message ?? "Cities not found",
                        data = new List<object>(),
                        success = false
                    });
                }

                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message = "Unexpected response from database",
                    data = new List<object>(),
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
                    data = new List<object>(),
                    success = false
                });
            }
        }
        #endregion

        #region Services

        [HttpGet("GetServicesDropDown")]
        public async Task<IActionResult> GetServices([FromQuery] string? search = null)
        {
            try
            {
                var result = await _dropDownRepo.GetServices(search);

                return result.StatusCode switch
                {
                    1 => Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = result.Message,
                        data = result.Data,
                        success = true
                    }),

                    2 => NotFound(new
                    {
                        status = false,
                        statusCode = 404,
                        message = result.Message,
                        data = Array.Empty<object>(),
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

        #endregion

        #region Brnaches

        [HttpGet("GetBranches")]
        public async Task<IActionResult> GetBranches()
        {
            try
            {

                var result = await _dropDownRepo.GetBranchesAsync(UserId);

                if (result.StatusCode == 0)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message,
                        data = result.Data,
                        success = false
                    });
                }

                if (!result.Data.Any())
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
                    data = new List<object>(),
                    success = false
                });
            }
        }
        #endregion

        #region User Types
        [HttpGet("GetUserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            try
            {
                var result = await _dropDownRepo.GetUserTypesAsync(UserId);

                if (result.StatusCode == 0)
                {
                    return StatusCode(500, new
                    {
                        status = false,
                        statusCode = 500,
                        message = result.Message,
                        data = result.Data,
                        success = false
                    });
                }

                if (!result.Data.Any())
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
                    data = new List<object>(),
                    success = false
                });
            }

        }
        #endregion

        #region Airlines

        [HttpGet("airlinesDropDown")]
        public async Task<IActionResult> GetAirlines()
        {
            try
            {
                var (statusCode, message, data) = await _dropDownRepo.GetAirlinesAsync();

                if (statusCode == 1)
                {
                    return Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message,
                        data,
                        success = true
                    });
                }

                if (statusCode == 2)
                {
                    return Ok(new
                    {
                        status = false,
                        statusCode = 404,
                        message,
                        data = Array.Empty<object>(),
                        success = false
                    });
                }

                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message,
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

        #region Airports

        [HttpGet("GetAirPortsDropDown")]
        public async Task<IActionResult> GetAirPortsDropDown()
        {
            try
            {
                var result = await _dropDownRepo.GetAirPortsAsync();

                return Ok(new
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
                    data = new List<object>(),
                    success = false
                });
            }
        }
        #endregion

        #region Payment Methods
        [HttpGet("GetPaymentMethodsDropDown")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            try
            {
                var result = await _dropDownRepo.GetPaymentMethods();

                if (result.ReturnValue == 1)
                {
                    return Ok(new
                    {
                        status = true,
                        statusCode = 200,
                        message = result.Message,
                        data = result.Data,
                        success = true
                    });
                }

                if (result.ReturnValue == 2)
                {
                    return Ok(new
                    {
                        status = false,
                        statusCode = 404,
                        message = result.Message,
                        data = Array.Empty<object>(),
                        success = false
                    });
                }

                return StatusCode(500, new
                {
                    status = false,
                    statusCode = 500,
                    message = result.Message,
                    data = Array.Empty<object>(),
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
                    data = Array.Empty<object>(),
                    success = false
                });
            }
        }
        #endregion

        #region Ticket Types
        [HttpGet("GetTicketTypesDropDown")]
        public async Task<IActionResult> GetTicketTypesDropDown()
        {
            try
            {
                var result = await _dropDownRepo.GetTicketTypesDropDownAsync();

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
                        data = new List<dynamic>(),
                        success = true
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
    }
}
