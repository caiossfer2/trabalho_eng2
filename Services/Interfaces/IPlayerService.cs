using api.Data.Dtos;
using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Services.Interfaces
{
    public interface IPlayerService
    {
        public ActionResult<List<PlayerDto>>? getAll();

        public Task<ActionResult<PlayerModel>?> getById(int id);

        public Task<ActionResult<PlayerModel>?> create([FromBody] SimplPlayerDTO playerDto);

        public Task<ActionResult<PlayerModel>?> update([FromBody] SimplPlayerDTO playerDto, int id);

        public Task<ActionResult<bool>?> delete(int id);
    }
}
