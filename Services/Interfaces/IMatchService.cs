using api.Data.Dtos;
using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Services.Interfaces
{
    public interface IMatchService
    {
        public Task<ActionResult<List<MatchResponseDTO>>?> getAll();

        public Task<ActionResult<MatchResponseDTO>?> getById(int id);

        public Task<ActionResult<bool>?> delete(int id);
        public Task<ActionResult<MatchModel>?> create([FromBody] MatchDTO match);

        public Task<ActionResult<MatchResponseDTO>?> update([FromBody] MatchDTO matchDTO, int id);
    }
}
