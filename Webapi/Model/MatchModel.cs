using System;

namespace Webapi.Model
{
    public class MatchModel
    {
        public MatchModel()
        {
            this.Players = new List<PlayerModel>();
        }
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public List<PlayerModel> Players { get; set; }
    }
}
