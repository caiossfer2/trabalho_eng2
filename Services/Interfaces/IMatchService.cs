using api.Data.Dtos;
using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Services.Interfaces
{
    public interface IMatchService
    {
        public Task<ActionResult<List<GetMatchDTO>>> getAll();

        public Task<ActionResult<GetMatchDTO>> getById(int id);

        public Task<ActionResult<bool>> delete(int id);
        public Task<ActionResult<GetMatchDTO>> create([FromBody] PostMatchDTO match);

        public Task<ActionResult<GetMatchDTO>> update([FromBody] PostMatchDTO matchDTO, int id);
    }
}
