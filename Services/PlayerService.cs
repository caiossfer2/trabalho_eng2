using api.Data;
using api.Data.Dtos;
using api.Model;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class PlayerService : IPlayerService
    {

        private readonly Context _context;

        public PlayerService(Context context)
        {
            _context = context;
        }
        public ActionResult<List<PlayerDto>>? getAll()
        {
            var players = _context.Players.Select(p => new
            {
                p.Id,
                p.Name,
                Matches = p.Matches != null ? p.Matches.Select(m => new { m.WinnerId, m.LoserId, m.Id }).ToList() : null
            }).ToList();

            if (players == null)
            {
                return null;
            }
            var playersDtos = players.ConvertAll(x => new PlayerDto
            {
                Id = x.Id,
                Name = x.Name,
                Matches = x.Matches != null ? x.Matches.ConvertAll(y => new SimplMatchDto
                {
                    Id = y.Id,
                    WinnerId = y.WinnerId,
                    LoserId = y.LoserId
                }) : null
            });

            return playersDtos;
        }

        public async Task<ActionResult<PlayerModel>?> getById(int id)
        {
            PlayerModel? player = await _context.Players.Include(x => x.Matches).FirstOrDefaultAsync(x => x.Id == id);
            if (player == null)
            {
                throw new ArgumentException("Player not found");
            }
            return player;
        }

        public async Task<ActionResult<PlayerModel>?> create([FromBody] SimplPlayerDTO playerDto)
        {
            if (playerDto.Name == null)
            {
                throw new ArgumentException("Name property must be sent");
            }

            PlayerModel playerModel = new()
            {
                Name = playerDto.Name
            };
            await _context.Players.AddAsync(playerModel);
            await _context.SaveChangesAsync();
            return playerModel;
        }

        public async Task<ActionResult<PlayerModel>?> update([FromBody] SimplPlayerDTO playerDto, int id)
        {
            if (playerDto.Name == null)
            {
                throw new ArgumentException("Name property must be sent");
            }

            PlayerModel? obtainedPlayer = await _context.Players.FindAsync(id);
            if (obtainedPlayer == null)
            {
                throw new ArgumentException("Player not found");
            }
            obtainedPlayer.Name = playerDto.Name;
            _context.Players.Update(obtainedPlayer);
            await _context.SaveChangesAsync();
            return obtainedPlayer;
        }

        public async Task<ActionResult<bool>?> delete(int id)
        {
            PlayerModel? obtainedPlayer = await _context.Players.FindAsync(id);
            if (obtainedPlayer == null)
            {
                throw new ArgumentException("Player not found");
            }
            _context.Players.Remove(obtainedPlayer);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
