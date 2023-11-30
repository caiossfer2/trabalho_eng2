using System;
using System.Net;
using Webapi.Data;
using Webapi.Data.Dtos;
using Webapi.Model;
using Webapi.Services;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<List<GetPlayerDTO>> getAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<GetPlayerDTO> players = _service.getAll()?.Value;
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
        public async Task<ActionResult<List<RankedPlayer>>> getRanking()
        {
            try
            {
                List<RankedPlayer> ranking = (await _service.getRanking())?.Value;
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
        public async Task<ActionResult<GetPlayerDTO>> getById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetPlayerDTO player = (await _service.getById(id))?.Value;
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
        public async Task<ActionResult<dynamic>> create([FromBody] PostPlayerDTO playerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = (await _service.create(playerDto))?.Value;
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
        public async Task<ActionResult<GetPlayerDTO>> update([FromBody] PostPlayerDTO playerDto, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetPlayerDTO player = (await _service.update(playerDto, id))?.Value;
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
                return Ok((await _service.delete(id))?.Value);
            }
            catch (ArgumentException e)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            };
        }
    }
}
