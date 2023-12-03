using System.Net;
using Webapi.Data.Dtos;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<GetPlayerDTO>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<GetPlayerDTO> players = _service.GetAll()?.Value;
                if (players == null)
                {
                    return NotFound();

                }
                return Ok(players);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("ranking")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PlayerWithWins>>> getRanking()
        {
            try
            {
                List<PlayerWithWins> ranking = (await _service.GetRanking())?.Value;
                if (ranking == null)
                {
                    return NotFound();

                }
                return Ok(ranking);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = (await _service.GetById(id))?.Value;
                if (player == null)
                {
                    return NotFound();
                }
                return Ok(player);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Create([FromBody] PostPlayerDTO playerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = (await _service.Create(playerDto))?.Value;
                if (player == null)
                {
                    return BadRequest();
                }
                return StatusCode(StatusCodes.Status201Created, player);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<GetPlayerDTO>> Update([FromBody] PostPlayerDTO playerDto, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetPlayerDTO player = (await _service.Update(playerDto, id))?.Value;
                if (player == null)
                {
                    return NotFound();
                }
                return Ok(player);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> delete(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok((await _service.Delete(id))?.Value);
            }
            catch (ArgumentException e)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            };
        }
    }
}
