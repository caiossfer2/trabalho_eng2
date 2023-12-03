using Webapi.Data.Dtos;
using Webapi.Data;
using Microsoft.AspNetCore.Mvc;
using Webapi.Model;
using Microsoft.EntityFrameworkCore;
using Webapi.Services.Interfaces;

namespace Webapi.Services
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
            PlayerModel playerModel = await _context.Players.FirstOrDefaultAsync(u => u.Username == player.Username && u.Password == player.Password
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
