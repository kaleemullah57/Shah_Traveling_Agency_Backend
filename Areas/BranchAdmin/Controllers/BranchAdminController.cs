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
                var result = await _branchAdminRepo.AddDestination(model, UserId,  BranchId );

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
                    return BadRequest(new
                    {
                        statusCode = 400,
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
        #endregion




    }
}
