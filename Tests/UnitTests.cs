using Newtonsoft.Json;
using Webapi.Data.Dtos;
using Webapi.Model;

namespace Tests;

public class UnitTests
{
    [Fact]
    public void GetNumberOfMatchesPlayed_ReturnsCorrectCount()
    {
        // Arrange
        var player = new PlayerModel();
        player.Matches.Add(new MatchModel());
        player.Matches.Add(new MatchModel());

        // Act
        int result = PlayerModel.GetNumberOfMatchesPlayed(player);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetNumberOfMatchesPlayed_ReturnsZeroForNewPlayer()
    {
        // Arrange
        var player = new PlayerModel();

        // Act
        int result = PlayerModel.GetNumberOfMatchesPlayed(player);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetNumberOfWins_ReturnsCorrectNumberOfWins()
    {
        // Arrange
        var player = new PlayerModel
        {
            Id = 2,
            Name = "Foo",
            Username = "Bar",
        };
        player.Matches.Add(new MatchModel
        {
            Id = 1,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 2,
            LoserId = 2,
            WinnerId = 1,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 1,
            WinnerId = 2,
        });

        // Act
        int result = PlayerModel.GetNumberOfWins(player);

        // Assert
        Assert.Equal(2, result);
    }


    [Fact]
    public void GetNumberOfWins_ReturnsZeroForNewPlayer()
    {
        // Arrange
        var player = new PlayerModel();

        // Act
        int result = PlayerModel.GetNumberOfWins(player);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetNumberOfWins_ReturnsZeroForPlayerThatOnlyLost()
    {
        // Arrange
        var player = new PlayerModel
        {
            Id = 1,
            Name = "Foo",
            Username = "Bar",
        };
        player.Matches.Add(new MatchModel
        {
            Id = 1,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 2,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 1,
            WinnerId = 2,
        });

        // Act
        int result = PlayerModel.GetNumberOfWins(player);

        // Assert
        Assert.Equal(0, result);
    }


    [Fact]
    public void AreIdsEqual_ReturnsTrueForEqualIds()
    {
        // Act
        var result = PlayerModel.AreIdsEqual(1, 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreIdsEqual_ReturnsFalseForDifferentIds()
    {
        // Act
        var result = PlayerModel.AreIdsEqual(1, 2);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void GetPercentageOfWins_ReturnsCorrectPercentage()
    {
        // Arrange
        var player = new PlayerModel
        {
            Id = 1,
            Name = "Foo",
            Username = "Bar",
        };
        player.Matches.Add(new MatchModel
        {
            Id = 1,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 2,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 2,
            WinnerId = 1,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 1,
            WinnerId = 2,
        });

        // Act
        var result = PlayerModel.GetPercentageOfWins(player);

        // Assert
        Assert.Equal(25, result);
    }


    [Fact]
    public void GetPercentageOfWins_ReturnsZeroForPlayerThatOnlyLost()
    {
        // Arrange
        var player = new PlayerModel
        {
            Id = 1,
            Name = "Foo",
            Username = "Bar",
        };
        player.Matches.Add(new MatchModel
        {
            Id = 1,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 2,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 1,
            WinnerId = 2,
        });
        player.Matches.Add(new MatchModel
        {
            Id = 3,
            LoserId = 1,
            WinnerId = 2,
        });

        // Act
        var result = PlayerModel.GetPercentageOfWins(player);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetPercentageOfWins_ReturnsZeroForNewPlayer()
    {
        // Arrange
        var player = new PlayerModel();

        // Act
        var result = PlayerModel.GetPercentageOfWins(player);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void PlayerAlreadyExists_ReturnsTrueForPlayerWithRepeatedUsername()
    {
        // Arrange
        var player1 = new PlayerModel { Username = "useruser" };
        List<GetPlayerDTO> players = new()
        {
            new GetPlayerDTO { Username = "user1" },
            new GetPlayerDTO { Username = "user2" },
            new GetPlayerDTO { Username = "user3" },
            new GetPlayerDTO { Username = "useruser" }
        };

        // Act
        var result = PlayerModel.PlayerAlreadyExists(player1, players);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void PlayerAlreadyExists_ReturnsFalseForPlayerWithUnpublishedUsername()
    {
        // Arrange
        var player1 = new PlayerModel { Username = "useruser" };
        List<GetPlayerDTO> players = new()
        {
            new GetPlayerDTO { Username = "user1" },
            new GetPlayerDTO { Username = "user2" },
            new GetPlayerDTO { Username = "user3" },
            new GetPlayerDTO { Username = "user4" }
        };

        // Act
        var result = PlayerModel.PlayerAlreadyExists(player1, players);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void PlayerAlreadyExists_ReturnsFalseForFirstPlayer()
    {
        // Arrange
        var player1 = new PlayerModel { Username = "useruser" };
        List<GetPlayerDTO> players = new();

        // Act
        var result = PlayerModel.PlayerAlreadyExists(player1, players);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void GetRanking_ReturnsCorrectRanking()
    {
        // Arrange
        PlayerModel p1 = new()
        {
            Id = 1,
            Name = "User1",
            Username = "user1",
            Matches = new List<MatchModel>
                {
                    new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },  new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },  new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },
                    new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    }, new MatchModel{
                        WinnerId = 3,
                        LoserId = 1
                    },
                }
        };

        PlayerModel p2 = new()
        {
            Id = 2,
            Name = "User2",
            Username = "user2",
            Matches = new List<MatchModel>
                {
                    new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },
                    new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },  new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },  new MatchModel{
                        WinnerId = 1,
                        LoserId = 2
                    },new MatchModel{
                        WinnerId = 3,
                        LoserId = 2
                    },new MatchModel{
                        WinnerId = 3,
                        LoserId = 2
                    },new MatchModel{
                        WinnerId = 2,
                        LoserId = 3
                    },
                }
        };


        PlayerModel p3 = new()
        {
            Id = 3,
            Name = "User3",
            Username = "user3",
            Matches = new List<MatchModel>
                {
                    new MatchModel{
                        WinnerId = 3,
                        LoserId = 2
                    },  new MatchModel{
                        WinnerId = 3,
                        LoserId = 2
                    },
                    new MatchModel{
                        WinnerId = 3,
                        LoserId = 2
                    },new MatchModel{
                        WinnerId = 2,
                        LoserId = 3
                    },
                }
        };

        List<PlayerModel> players = new()
        {
            p1,p2,p3
        };

        // Act
        var result = PlayerModel.GetRanking(players);

        // Assert

        PlayerWithWins pCorrect1 = new PlayerWithWins
        {
            Player = new SimplPlayerDTO
            {
                Id = 1,
                Name = "User1",
                Username = "user1",
            },
            Wins = 4
        };
        PlayerWithWins pCorrect2 = new PlayerWithWins
        {
            Player = new SimplPlayerDTO
            {
                Id = 3,
                Name = "User3",
                Username = "user3",
            },
            Wins = 3
        };
        PlayerWithWins pCorrect3 = new PlayerWithWins
        {
            Player = new SimplPlayerDTO
            {
                Id = 2,
                Name = "User2",
                Username = "user2",
            },
            Wins = 1
        };

        Assert.Equal(JsonConvert.SerializeObject(result[0]), JsonConvert.SerializeObject(pCorrect1));
        Assert.Equal(JsonConvert.SerializeObject(result[1]), JsonConvert.SerializeObject(pCorrect2));
        Assert.Equal(JsonConvert.SerializeObject(result[2]), JsonConvert.SerializeObject(pCorrect3));
    }

    [Fact]
    public void getBiggestRival_Returns()
    {
        // Arrange
        PlayerModel player1 = new()
        {
            Id = 1,
            Name = "Player1",
            Username = "player1",
        };

        PlayerModel player2 = new()
        {
            Id = 2,
            Name = "Player2",
            Username = "player2",
        };

        PlayerModel player3 = new()
        {
            Id = 3,
            Name = "Player3",
            Username = "player3",
        };

        PlayerModel player = new()
        {
            Id = 1,
            Name = "Player1",
            Username = "player1",
            Matches = new List<MatchModel>
                {
                    new MatchModel{
                        WinnerId = 2,
                        LoserId = 1,
                        Players = new List<PlayerModel>{
                            player1, player2
                        }
                    },
                    new MatchModel{
                        WinnerId = 2,
                        LoserId = 1,
                        Players = new List<PlayerModel>{
                            player1, player2
                        }
                    },
                    new MatchModel{
                        WinnerId = 1,
                        LoserId = 2,
                        Players = new List<PlayerModel>{
                            player1, player2
                        }
                    },
                    new MatchModel{
                        WinnerId = 3,
                        LoserId = 1,
                        Players = new List<PlayerModel>{
                            player1, player3
                        }
                    },
                    new MatchModel{
                        WinnerId = 3,
                        LoserId = 1,
                        Players = new List<PlayerModel>{
                            player1, player3
                        }
                    },
                    new MatchModel{
                        WinnerId = 3,
                        LoserId = 1,
                        Players = new List<PlayerModel>{
                            player1, player3
                        }
                    },
                }
        };


        // Act
        var result = PlayerModel.getBiggestRival(player);

        // Assert
        Assert.Equal("player3", result.Username);
    }

    [Fact]
    public void GetRanking_ReturnsCorrectOrder()
    {
        // Arrange
        var players = new List<PlayerModel>
        {
            new PlayerModel { Id = 1, Name = "Player 1", Username = "player1" },
            new PlayerModel { Id = 2, Name = "Player 2", Username = "player2" },
            new PlayerModel { Id = 3, Name = "Player 3", Username = "player3" }
        };

        players[0].Matches.Add(new MatchModel { WinnerId = 1 });
        players[0].Matches.Add(new MatchModel { WinnerId = 1 });
        players[1].Matches.Add(new MatchModel { WinnerId = 2 });
        players[1].Matches.Add(new MatchModel { WinnerId = 2 });
        players[1].Matches.Add(new MatchModel { WinnerId = 1 });
        players[2].Matches.Add(new MatchModel { WinnerId = 3 });

        // Act
        List<PlayerWithWins> result = PlayerModel.GetRanking(players);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Player 1", result[0].Player.Name);
        Assert.Equal("Player 2", result[1].Player.Name);
        Assert.Equal("Player 3", result[2].Player.Name);
    }




}