using System;

namespace Webapi.Data.Dtos
{
    public class GetPlayerByIdDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfMatches { get; set; }
        public double PercentageOfWins { get; set; }
        public SimplPlayerDTO BiggestRival {get;set;}
    }
}
