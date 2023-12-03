using Webapi.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<ActionResult<dynamic>> Authenticate(LoginDTO loginDTO);
    }
}
