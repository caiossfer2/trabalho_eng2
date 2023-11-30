using System;

namespace Webapi.Model
{
    public class PlayerModel
    {

        public PlayerModel()
        {
            this.Matches = new();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<MatchModel> Matches { get; set; }
    }
}
