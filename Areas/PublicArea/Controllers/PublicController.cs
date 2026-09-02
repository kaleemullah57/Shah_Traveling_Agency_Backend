using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Controllers;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories;
using Shah_Traveling_Agency_API.Areas.PublicArea.Repositories;

namespace Shah_Traveling_Agency_API.Areas.PublicArea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : BaseController
    {

        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        private readonly PublicRepo _publicRepo;

        public PublicController(JwtService jwtService, PasswordService passwordService, PublicRepo PublicRepo)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _publicRepo = PublicRepo;
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
    }
}
