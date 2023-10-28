using System;
using System.Net;
using api.Data;
using api.Data.Dtos;
using api.Model;
using api.Services;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
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
        public ActionResult<List<PlayerDto>> getAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               List<PlayerDto>? players = _service.getAll()?.Value;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerModel>> getById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PlayerModel? player = (await _service.getById(id))?.Value;
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
        public async Task<ActionResult<PlayerModel>> create([FromBody] SimplPlayerDTO playerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PlayerModel? player = (await _service.create(playerDto))?.Value;
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
        public async Task<ActionResult<PlayerModel>> update([FromBody] SimplPlayerDTO playerDto, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PlayerModel? player = (await _service.update(playerDto, id))?.Value;
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
