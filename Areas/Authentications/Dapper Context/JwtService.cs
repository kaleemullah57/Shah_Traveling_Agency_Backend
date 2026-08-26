using Microsoft.IdentityModel.Tokens;
using Shah_Traveling_Agency_API.Areas.Authentications.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shah_Traveling_Agency_API.Areas.Authentications.Dapper_Context
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(LoginUser user)
        {
            var claims = new List<Claim>
            {
                //new Claim(
                //    ClaimTypes.NameIdentifier,
                //    user.UserID.ToString()
                //),
                new Claim(
                    "UserId",
                    user.UserID.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.UserType
                ),

                new Claim(
                    "UserTypeId",
                    user.UserTypeId.ToString()
                ),

                new Claim(
                    "BranchId",
                    user.BranchId.ToString()
                )
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _config["Jwt:ExpireMinutes"]
                    )
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


    }
}
