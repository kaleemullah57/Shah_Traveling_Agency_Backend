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



        #endregion




    }
}
