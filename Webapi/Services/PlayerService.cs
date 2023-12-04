using Webapi.Data;
using Webapi.Data.Dtos;
using Webapi.Model;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Webapi.Services
{
    public class PlayerService : IPlayerService
    {

        private readonly Context _context;

        public PlayerService(Context context)
        {
            _context = context;
        }
        public ActionResult<List<GetPlayerDTO>> GetAll()
        {
            var players = _context.Players.Select(p => new
            {
                p.Id,
                p.Name,
                p.Username,
                Matches = p.Matches != null ? p.Matches.Select(m => new { m.WinnerId, m.LoserId, m.Id }).ToList() : null
            }).ToList();

            if (players == null)
            {
                return null;
            }
            var playersDtos = players.ConvertAll(x => new GetPlayerDTO
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.Username,
                Matches = x.Matches != null ? x.Matches.ConvertAll(y => new SimplMatchDto
                {
                    Id = y.Id,
                    WinnerId = y.WinnerId,
                    LoserId = y.LoserId
                }) : null
            });

            return playersDtos;
        }

        public async Task<ActionResult<List<PlayerWithWins>>> GetRanking()
        {
            List<PlayerModel> players = await _context.Players.Include(x => x.Matches).ToListAsync();
            return PlayerModel.GetRanking(players);
        }

        public async Task<ActionResult<GetPlayerByIdDTO>> GetById(int id)
        {
            PlayerModel player = await _context.Players.Include(x => x.Matches).FirstOrDefaultAsync(x => x.Id == id);
            if (player == null)
            {
                throw new ArgumentException("Player not found");
            }

            PlayerModel biggestRivalPlayerModel = await _context.Players.Include(x => x.Matches).FirstOrDefaultAsync(x => x.Id == PlayerModel.GetBiggestRival(player));
            SimplPlayerDTO biggestRival = new(){
                Id = -1,
                Name = "nobody",
                Username = "nobody",
            };

            if(biggestRivalPlayerModel != null){
                biggestRival.Id = biggestRivalPlayerModel.Id;
                biggestRival.Name = biggestRivalPlayerModel.Name;
                biggestRival.Username = biggestRivalPlayerModel.Username;
            }
            

            return new GetPlayerByIdDTO
            {
                Id = player.Id,
                Username = player.Username,
                Name = player.Name,
                NumberOfWins = PlayerModel.GetNumberOfWins(player),
                NumberOfMatches = PlayerModel.GetNumberOfMatchesPlayed(player),
                PercentageOfWins = PlayerModel.GetPercentageOfWins(player),
                BiggestRival = biggestRival,
            };
        }

        public async Task<ActionResult<CreatePlayerReturnDTO>> Create([FromBody] PostPlayerDTO playerDto)
        {
            if (playerDto.Name == null)
            {
                throw new ArgumentException("Name property must be sent");
            }

            List<GetPlayerDTO> players = GetAll().Value;

            PlayerModel playerModel = new()
            {
                Name = playerDto.Name,
                Username = playerDto.Username,
                Password = playerDto.Password,
            };


            if (PlayerModel.PlayerAlreadyExists(playerModel, players))
            {
                throw new ArgumentException("Username already exists");
            }


            await _context.Players.AddAsync(playerModel);
            await _context.SaveChangesAsync();
            return new CreatePlayerReturnDTO
            {
                Username = playerDto.Username,
                Name = playerDto.Name,
            };
        }

        public async Task<ActionResult<GetPlayerDTO>> Update([FromBody] PostPlayerDTO playerDto, int id)
        {
            if (playerDto.Name == null)
            {
                throw new ArgumentException("Name property must be sent");
            }
            if (playerDto.Username == null)
            {
                throw new ArgumentException("Username property must be sent");
            }

            PlayerModel obtainedPlayer = await _context.Players.FindAsync(id);
            if (obtainedPlayer == null)
            {
                throw new ArgumentException("Player not found");
            }
            obtainedPlayer.Name = playerDto.Name;
            obtainedPlayer.Username = playerDto.Username;
            obtainedPlayer.Password = playerDto.Password;
            _context.Players.Update(obtainedPlayer);
            await _context.SaveChangesAsync();
            return convertPlayerModelToGetPlayer(obtainedPlayer);
        }

        public async Task<ActionResult<bool>> Delete(int id)
        {
            PlayerModel obtainedPlayer = await _context.Players.FindAsync(id);
            if (obtainedPlayer == null)
            {
                throw new ArgumentException("Player not found");
            }
            _context.Players.Remove(obtainedPlayer);
            await _context.SaveChangesAsync();
            return true;
        }


        private GetPlayerDTO convertPlayerModelToGetPlayer(PlayerModel playerModel)
        {
            return new GetPlayerDTO
            {
                Name = playerModel.Name,
                Username = playerModel.Username,
                Id = playerModel.Id,
                Matches = playerModel.Matches.ConvertAll(m => new SimplMatchDto
                {
                    Id = m.Id,
                    LoserId = m.LoserId,
                    WinnerId = m.WinnerId,
                    Players = m.Players.ConvertAll(p => new SimplPlayerDTO
                    {
                        Name = p.Name,
                        Id = p.Id,
                        Username = p.Username
                    })
                })
            };
        }

    }
}