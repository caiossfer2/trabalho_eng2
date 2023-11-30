using System;

namespace Webapi.Data.Dtos
{
    public class GetPlayerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public List<SimplMatchDto> Matches { get; set; }
    }
}