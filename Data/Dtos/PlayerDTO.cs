using System;

namespace api.Data.Dtos
{
    public class PlayerDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<SimplMatchDto>? Matches {get;set;}
    }
}