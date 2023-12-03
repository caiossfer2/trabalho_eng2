using Webapi.Data.Dtos;
using Webapi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Services.Interfaces
{
    public interface IMatchService
    {
        public Task<ActionResult<List<GetMatchDTO>>> GetAll();

        public Task<ActionResult<GetMatchDTO>> GetById(int id);

        public Task<ActionResult<bool>> Delete(int id);
        public Task<ActionResult<GetMatchDTO>> Create([FromBody] PostMatchDTO match);

        public Task<ActionResult<GetMatchDTO>> Update([FromBody] PostMatchDTO matchDTO, int id);
    }
}
