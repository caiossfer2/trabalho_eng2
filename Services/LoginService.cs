using api.Data.Dtos;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Model;
using Microsoft.EntityFrameworkCore;
using api.Services.Interfaces;

namespace api.Services
{
    public class LoginService : ILoginService
    {
        private readonly Context _context;
        public LoginService(Context context)
        {
            _context = context;
        }

        public async Task<ActionResult<dynamic>> Authenticate(LoginDTO player)
        {
            PlayerModel? playerModel = await _context.Players.FirstOrDefaultAsync(u => u.Username == player.Username && u.Password == player.Password
            );

            if (playerModel == null)
            {
                throw new ArgumentException("Dados de login incorretos");
            }

            var token = TokenService.GenerateToken(playerModel);

            return new
            {
                playerModel.Username,
                playerModel.Name,
                jwt = token
            };
        }
    }
}
