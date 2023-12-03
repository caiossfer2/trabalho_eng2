using System;
using System.Net;
using System.Numerics;
using Webapi.Data;
using Webapi.Data.Dtos;
using Webapi.Model;
using Webapi.Services;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Webapi.Controllers
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
        [Authorize]
        public async Task<ActionResult<List<GetMatchDTO>>> getAll()
        {
            try
            {
                List<GetMatchDTO> matches = (await _service.GetAll())?.Value;
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
        [Authorize]
        public async Task<ActionResult<GetMatchDTO>> getById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetMatchDTO match = (await _service.GetById(id))?.Value;
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
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<GetMatchDTO>> create([FromBody] PostMatchDTO match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetMatchDTO matchModel = (await _service.Create(match))?.Value;
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
        [Authorize]
        public async Task<ActionResult<GetMatchDTO>> update([FromBody] PostMatchDTO matchDTO, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                GetMatchDTO obtainedmatch = (await _service.Update(matchDTO, id))?.Value;
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
