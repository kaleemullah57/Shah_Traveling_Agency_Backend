using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Shah_Traveling_Agency_API.Areas.Authentications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int UserId =>
            int.TryParse(User.FindFirstValue("UserId"), out var userId)
                ? userId
                : 0;

        protected int BranchId =>
            int.TryParse(User.FindFirstValue("BranchId"), out var branchId)
                ? branchId
                : 0;

        protected int UserTypeId =>
            int.TryParse(User.FindFirstValue("UserTypeId"), out var userTypeId)
                ? userTypeId
                : 0;

        protected string Email =>
            User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }
}
