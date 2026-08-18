using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context;
using Shah_Traveling_Agency_API.Areas.Authentications.Models;
using Shah_Traveling_Agency_API.Areas.Authentications.Repositories;

namespace Shah_Traveling_Agency_API.Areas.Authentications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly AuthRepo _authRepo;
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;
        public AuthController(AuthRepo authRepo,JwtService jwtService, PasswordService passwordService)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }




        #region Register Users

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working");
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            try
            {
                var result = await _authRepo.RegisterUser(request);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Login User

        // Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var user = await _authRepo.Login(request);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        status = "Failed",
                        statusCode = 401,
                        message = "Email or Password Incorrect"
                    });
                }

                if (user == null || string.IsNullOrEmpty(user.Password))
                {
                    return Unauthorized(new
                    {
                        status = "Failed",
                        statusCode = 401,
                        message = "Email or Password Incorrect"
                    });
                }

                var passwordValid = _passwordService.VerifyPassword(request.Password, user.Password);

                if (!passwordValid)
                {
                    return Unauthorized(new
                    {
                        status = "Failed",
                        statusCode = 401,
                        message = "Email or Password Incorrect"
                    });
                }

                var token = _jwtService.GenerateToken(user);

                var response = new LoginResponse
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserTypeId = user.UserTypeId,
                    UserType = user.UserType,
                    BranchId = user.BranchId,
                    Token = token
                };

                return Ok(new
                {
                    status = "Ok",
                    statusCode = 200,
                    message = "User LoggedIn Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Error",
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
