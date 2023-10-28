using System;

namespace api.Data.Dtos
{
    public class MatchDTO
    {

        public int? Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
    }
}