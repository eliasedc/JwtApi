using JwtApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            List<UserModel> lstUserSystem = new List<UserModel>()
            {
                new UserModel(){ Username = "James", Password = "pwdEliasdc", Function = "Admin", ApiClientName = "ClientOne"/* Aqui poderia ter ainda inforamções para validações ainda mais personalizadas, 
                                                                                                                                  * para criar Claims persoanlizadas. 
                                                                                                                                  * Ex.: , CustomClaim = "CustomClaimOne"*/ },
                new UserModel(){ Username = "Emily", Password = "pwdEliasdc", Function = "Common", ApiClientName = "ClientOne" },
                new UserModel(){ Username = "Michael", Password = "pwdEliasdc", Function = "Admin", ApiClientName = "ClientTwo" },
                new UserModel(){ Username = "Sarah", Password = "pwdEliasdc", Function = "Common", ApiClientName = "ClientTwo" },
            };

            UserModel? user = lstUserSystem.FirstOrDefault(r => r.Username == model.Username &&
                                                                r.Password == model.Password);

            if(user == null)
            {
                return Unauthorized();
            }
                        
            string? jwtKey = _configuration["Jwt:Key"]; // Same from program.cs
            string? jwtIssuer = _configuration["Jwt:Issuer"]; // Same from program.cs

            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(jwtKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, user.Function),
                    //Aqui posso adicionar também claim personalizados ex.:
                    //new Claim("customclaim", user.CustomClaim)
                    //new Claim("permission", user.CustomClaim)
                    new Claim("TestClaim", "ValueTestClaim")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = user.ApiClientName, 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            string token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = token });
            

        }
    }
}
