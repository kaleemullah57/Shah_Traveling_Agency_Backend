using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.DropDown.Repositories;
using Shah_Traveling_Agency_API.Areas.PublicArea.Repositories;

namespace Shah_Traveling_Agency_API.Areas.DropDown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DropDownController : ControllerBase
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
        public async Task<IActionResult> GetProvincesByCountryId([FromQuery] int countryId)
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


        
    }
}
