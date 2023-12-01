using Webapi.Data.Dtos;
using Webapi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Services.Interfaces
{
    public interface IPlayerService
    {
        public ActionResult<List<GetPlayerDTO>> getAll();

        public Task<ActionResult<GetPlayerDTO>> getById(int id);

        public Task<ActionResult<dynamic>> create([FromBody] PostPlayerDTO playerDto);

        public Task<ActionResult<GetPlayerDTO>> update([FromBody] PostPlayerDTO playerDto, int id);

        public Task<ActionResult<bool>> delete(int id);

        public Task<ActionResult<List<PlayerWithWins>>> getRanking();

    }
}
