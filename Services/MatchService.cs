using api.Data.Dtos;
using api.Data;
using api.Model;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.RegularExpressions;

namespace api.Services
{
    public class MatchService : IMatchService
    {
        private readonly Context _context;

        public MatchService(Context context)
        {
            _context = context;
        }

        public async Task<ActionResult<List<MatchResponseDTO>>?> getAll()
        {
            List<MatchModel> matches = await _context.Matches.Include(p => p.Players).ToListAsync();
            var matchesDtos = matches.ConvertAll(x => new MatchResponseDTO
            {
                Id = x.Id,
                Winner = conversion(x.Players.Find(p => p.Id == x.WinnerId)),
                Loser = conversion(x.Players.Find(p => p.Id == x.LoserId)),
            });
            return matchesDtos;
        }

        public async Task<ActionResult<MatchResponseDTO>?> getById(int id)
        {
            MatchModel? match = await _context.Matches.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (match == null)
            {
                throw new ArgumentException("Match not found");
            }

            MatchResponseDTO? response = mainconversion(match);
            if (response == null)
            {
                return null;
            }
            return response;
        }

        public async Task<ActionResult<bool>?> delete(int id)
        {
            MatchModel? obtainedmatch = await _context.Matches.FindAsync(id);
            if (obtainedmatch == null)
            {
                throw new ArgumentException("Match not found");
            }
            _context.Matches.Remove(obtainedmatch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<MatchModel>?> create([FromBody] MatchDTO match)
        {

            if (match.LoserId != match.WinnerId)
            {
                if (match.LoserId > 0 && match.WinnerId > 0)
                {
                    PlayerModel? winner = await _context.Players.FirstOrDefaultAsync(x => x.Id == match.WinnerId);
                    PlayerModel? loser = await _context.Players.FirstOrDefaultAsync(x => x.Id == match.LoserId);
                    if (winner != null && loser != null)
                    {
                        MatchModel matchModel = new MatchModel
                        {
                            WinnerId = match.WinnerId,
                            LoserId = match.LoserId
                        };
                        matchModel.Players.Add(winner);
                        matchModel.Players.Add(loser);
                        await _context.Matches.AddAsync(matchModel);
                        await _context.SaveChangesAsync();
                        return matchModel;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new ArgumentException("Players ids must be greather than zero");
                }
            }
            else
            {
                throw new ArgumentException("Players ids must be different");
            }
        }


        public async Task<ActionResult<MatchResponseDTO>?> update([FromBody] MatchDTO matchDto, int id)
        {
            if (matchDto.WinnerId < 0 || matchDto.WinnerId < 0)
            {
                throw new ArgumentException("Players ids must be greather than 0");
            }

            if (matchDto.LoserId == matchDto.WinnerId)
            {
                throw new ArgumentException("Players ids must be different");
            }

            MatchModel? obtainedmatch = await _context.Matches.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (obtainedmatch == null)
            {
                throw new ArgumentException("Match not found");
            }

            PlayerModel? winner = await _context.Players.FirstOrDefaultAsync(x => x.Id == matchDto.WinnerId);
            PlayerModel? playerInList = obtainedmatch.Players.Find(p => p.Id == matchDto.WinnerId);
            if (playerInList == null && winner != null)
            {
                obtainedmatch.Players.Add(winner);
            }

            PlayerModel? loser = await _context.Players.FirstOrDefaultAsync(x => x.Id == matchDto.LoserId);
            playerInList = obtainedmatch.Players.Find(p => p.Id == matchDto.LoserId);
            if (playerInList == null && loser != null)
            {
                obtainedmatch.Players.Add(loser);
            }


            obtainedmatch.WinnerId = matchDto.WinnerId;
            obtainedmatch.LoserId = matchDto.LoserId;
            MatchResponseDTO? response = mainconversion(obtainedmatch);
            if (response == null)
            {
                throw new ArgumentException("Error occurred in conversion of data types");
            }
            _context.Matches.Update(obtainedmatch);
            await _context.SaveChangesAsync();
            return response;
        }

        private SimplPlayerDTO? conversion(PlayerModel? playerModel)
        {
            if (playerModel == null)
            {
                return null;
            }
            SimplPlayerDTO responseDto = new SimplPlayerDTO();
            responseDto.Name = playerModel.Name;
            responseDto.Id = playerModel.Id;
            return responseDto;
        }

        private MatchResponseDTO? mainconversion(MatchModel matchModel)
        {
            MatchResponseDTO response = new MatchResponseDTO();
            PlayerModel? winner = matchModel.Players.Find(p => p.Id == matchModel.WinnerId);
            PlayerModel? loser = matchModel.Players.Find(p => p.Id == matchModel.LoserId);
            if (winner != null && loser != null)
            {
                response.Winner = conversion(winner);
                response.Loser = conversion(loser);
                return response;
            }
            return null;
        }   
    }
}
