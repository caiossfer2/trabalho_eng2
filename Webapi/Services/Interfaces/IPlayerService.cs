using Webapi.Data.Dtos;
using Webapi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Services.Interfaces
{
    public interface IPlayerService
    {
        public ActionResult<List<GetPlayerDTO>> GetAll();

        public Task<ActionResult<GetPlayerByIdDTO>> GetById(int id);

        public Task<ActionResult<CreatePlayerReturnDTO>> Create([FromBody] PostPlayerDTO playerDto);

        public Task<ActionResult<GetPlayerDTO>> Update([FromBody] PostPlayerDTO playerDto, int id);

        public Task<ActionResult<bool>> Delete(int id);

        public Task<ActionResult<List<PlayerWithWins>>> GetRanking();

    }
}
