using System;
using System.Net;
using System.Numerics;
using api.Data;
using api.Data.Dtos;
using api.Model;
using api.Services;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {

        private readonly IMatchService _service;

        public MatchController(IMatchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MatchResponseDTO>>> getAll()
        {
            try
            {
                List<MatchResponseDTO>? matches = (await _service.getAll())?.Value;
                if (matches == null)
                {
                    return NotFound();
                }

                return Ok(matches);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResponseDTO>> getById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                MatchResponseDTO? match = (await _service.getById(id))?.Value;
                if (match == null)
                {
                    return NotFound();
                }
                return Ok(match);
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
            }
        }

        [HttpPost]
        public async Task<ActionResult<MatchModel>> create([FromBody] MatchDTO match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                MatchModel? matchModel = (await _service.create(match))?.Value;
                if (matchModel == null)
                {
                    return BadRequest();
                }

                return StatusCode(StatusCodes.Status201Created, matchModel);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MatchResponseDTO>> update([FromBody] MatchDTO matchDTO, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                MatchResponseDTO? obtainedmatch = (await _service.update(matchDTO, id))?.Value;
                if (obtainedmatch == null)
                {
                    return NotFound();
                }

                return Ok(obtainedmatch);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
