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
            if (player.Matches == null) return 0;
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
            double numberOfMatches = GetNumberOfMatchesPlayed(player);
            if(numberOfMatches == 0) return 0;
            return (GetNumberOfWins(player) / numberOfMatches) * 100;
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



        public static List<PlayerWithWins> GetRanking(List<PlayerModel> players)
        {
            List<PlayerWithWins> ranking = new List<PlayerWithWins>();

            foreach (PlayerModel player in players)
            {
                int wins = GetNumberOfWins(player);
                ranking.Add(new PlayerWithWins { Player = new SimplPlayerDTO { Id = player.Id, Name = player.Name, Username = player.Username }, Wins = wins });
            }

            return ranking.OrderByDescending(p => p.Wins).ToList();
        }


        public static SimplPlayerDTO getBiggestRival(PlayerModel player)
        {
            List<PlayerWithWins> players = new();

            foreach (MatchModel match in player.Matches)
            {
                if (match.LoserId == player.Id)
                {
                    foreach (PlayerModel matchPlayer in match.Players)
                    {
                        if (match.WinnerId == matchPlayer.Id)
                        {
                            PlayerModel winner = matchPlayer;
                            players.Add(new PlayerWithWins { Player = new SimplPlayerDTO { Id = winner.Id, Name = winner.Name, Username = winner.Username }, Wins = 0 });
                        }
                    }
                }
            }

            foreach (PlayerWithWins playerWithWins in players)
            {
                foreach (MatchModel match in player.Matches)
                {
                    if (playerWithWins.Player.Id == match.WinnerId)
                    {
                        playerWithWins.Wins++;
                    }
                }
            }

            if (players.Count == 0)
            {
                return new SimplPlayerDTO{ Id = -1, Name = "Nobody", Username = "nobody" };
            }

            return players.OrderByDescending(p => p.Wins).ToList()[0].Player;
        }

    }
}
