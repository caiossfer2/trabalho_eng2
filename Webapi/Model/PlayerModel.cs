using System;
using Webapi.Data.Dtos;

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



        public static int GetNumberOfMatchesPlayed(PlayerModel player)
        {
            return player.Matches.Count;
        }
        public static int GetNumberOfWins(PlayerModel player)
        {
            int counter = 0;
            foreach (MatchModel match in player.Matches)
            {
                if (match.WinnerId == player.Id)
                {
                    counter++;
                }
            }

            return counter;
        }
        public static double GetPercentageOfWins(PlayerModel player)
        {
            int counter = 0;
            foreach (MatchModel match in player.Matches)
            {
                if (match.WinnerId == player.Id)
                {
                    counter++;
                }
            }

            int wins = GetNumberOfWins(player);

            return (double)wins / counter;
        }

        public static bool AreIdsEqual(int id1, int id2)
        {
            return id1 == id2;
        }

        public static bool PlayerAlreadyExists(PlayerModel candidatePlayer, List<GetPlayerDTO> players)
        {

            foreach (GetPlayerDTO player in players)
            {
                if (player.Username == candidatePlayer.Username)
                {
                    return true;
                }
            }
            return false;
        }

     

        public static List<RankedPlayer> GetRanking(List<PlayerModel> players)
        {

            List<RankedPlayer> ranking = new List<RankedPlayer>();

            foreach (PlayerModel player in players)
            {
                int wins = GetNumberOfWins(player);
                System.Console.WriteLine(wins);
                ranking.Add(new RankedPlayer { player = new SimplPlayerDTO { Id = player.Id, Name = player.Name, Username = player.Username }, Wins = wins });
            }

            return ranking.OrderByDescending(p => p.Wins).ToList();
        }

    }
}
