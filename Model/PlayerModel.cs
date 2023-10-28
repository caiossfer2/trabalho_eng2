using System;

namespace api.Model
{
    public class PlayerModel
    {

        public PlayerModel()
        {
            this.Matches = new();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public List<MatchModel>? Matches { get;set;}
    }
}
