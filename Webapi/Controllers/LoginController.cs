using Webapi.Data.Dtos;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> login([FromBody] LoginDTO loginDTO)
        {

            try
            {
                var player = (await _loginService.Authenticate(loginDTO))?.Value;
                if (player == null)
                {
                    return BadRequest();
                }
                return Ok(player);

            }
            catch (ArgumentException e)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
