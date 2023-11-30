using api.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace api.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<ActionResult<dynamic>> Authenticate(LoginDTO loginDTO);
    }
}
