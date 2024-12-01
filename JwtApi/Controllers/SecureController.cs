using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet("systemuser-by-anotation-x")]
        [Authorize(Policy = "ApiClientOnePolicy", Roles = "Admin")]
        public IActionResult GetUsersDataX()
        {
            var testClaim = User.FindFirstValue("TestClaim");
            return Ok(new { Message = "Yes you can access users X!" });
        }

        [HttpGet("basicdata-by-anotation-x")]
        [Authorize(Policy = "ApiClientOnePolicy", Roles = "Admin, Common")]
        public IActionResult GetBasicsDataX()
        {
            return Ok(new { Message = "Yes you can access basic data X!" });
        }

        [HttpGet("systemuser-by-anotation-y")]
        [Authorize(Policy = "ApiClientTwoPolicy", Roles = "Admin")]
        public IActionResult GetUsersDataY()
        {
            return Ok(new { Message = "Yes you can access users Y!" });
        }

        [HttpGet("basicdata-by-anotation-y")]
        [Authorize(Policy = "ApiClientTwoPolicy", Roles = "Admin, Common")]
        public IActionResult GetBasicsDataY()
        {
            return Ok(new { Message = "Yes you can access basic data Y!" });
        }

    }
}
