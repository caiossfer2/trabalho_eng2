using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webapi.Data;
using Webapi.Data.Dtos;
using Webapi.Model;
using Webapi.Services;
using Xunit.Abstractions;

namespace Tests;

public class IntegrationTests
{

    private readonly DbContextOptions<Context> _options;

    public IntegrationTests()
    {
        _options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "MyDb")
            .Options;
    }

    [Fact]
    public async void GetById_ReturnsPlayer_WhenPlayerExists()
    {
        // Arrange
        using (var context = new Context(_options))
        {
            var player = new PlayerModel { Username = "testuser", Name = "Test User", Password = "aaa" };
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act
            var result = await service.GetById(1);

            // Assert
            var okResult = Assert.IsType<ActionResult<GetPlayerByIdDTO>>(result);
            GetPlayerByIdDTO player = okResult.Value;
            Assert.Equal("testuser", player.Username);
            Assert.Equal("Test User", player.Name);
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void GetById_ThrowsArgumentException_WhenPlayerDoesNotExist()
    {
        // Arrange
        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetById(17));
            context.Database.EnsureDeleted();
        }
    }


    [Fact]
    public async void Create_ReturnsPlayer_WhenPlayerDoesNotExist()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Name = "Test User", Username = "testuser", Password = "password" };

        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act
            var result = await service.Create(playerDto);

            // Assert
            var okResult = Assert.IsType<ActionResult<CreatePlayerReturnDTO>>(result);
            var player = Assert.IsType<CreatePlayerReturnDTO>(okResult.Value);
            Assert.Equal("testuser", player.Username);
            Assert.Equal("Test User", player.Name);
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void Create_ThrowsArgumentException_WhenPlayerAlreadyExists()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Name = "Test User", Username = "testuser", Password = "password" };

        using (var context = new Context(_options))
        {
            var existingPlayer = new PlayerModel { Name = "Test User", Username = "testuser", Password = "password" };
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();

            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Create(playerDto));
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void Create_ThrowsArgumentException_WhenNameIsNotProvided()
    {
        // Arrange
        var playerDto = new PostPlayerDTO {Username = "testuserrr", Password = "password" };

        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Create(playerDto));
            context.Database.EnsureDeleted();
        }
    }


    [Fact]
    public async void Update_ReturnsUpdatedPlayer_WhenPlayerExists()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Name = "Updated User", Username = "updateduser", Password = "newpassword" };

        using (var context = new Context(_options))
        {
            var existingPlayer = new PlayerModel { Id = 1, Name = "Test User", Username = "testuser", Password = "password" };
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();
        }

        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act
            var result = await service.Update(playerDto, 1);

            // Assert
            var okResult = Assert.IsType<ActionResult<GetPlayerDTO>>(result);
            var updatedPlayer = Assert.IsType<GetPlayerDTO>(okResult.Value);
            Assert.Equal(1, updatedPlayer.Id);
            Assert.Equal("updateduser", updatedPlayer.Username);
            Assert.Equal("Updated User", updatedPlayer.Name);
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void Update_ThrowsArgumentException_WhenPlayerDoesNotExist()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Name = "Updated User", Username = "updateduser", Password = "newpassword" };

        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Update(playerDto, 230));
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void Update_ThrowsArgumentException_WhenNameIsNotProvided()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Username = "updateduser", Password = "newpassword" };

        using (var context = new Context(_options))
        {
            var existingPlayer = new PlayerModel { Id = 1, Name = "Test User", Username = "testuser", Password = "password" };
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();

            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Update(playerDto, 1));
            context.Database.EnsureDeleted();
        }
    }

    [Fact]
    public async void Update_ThrowsArgumentException_WhenUsernameIsNotProvided()
    {
        // Arrange
        var playerDto = new PostPlayerDTO { Name = "Updated User", Password = "newpassword" };

        using (var context = new Context(_options))
        {
            var existingPlayer = new PlayerModel { Id = 1, Name = "Test User", Username = "testuser", Password = "password" };
            context.Players.Add(existingPlayer);
            await context.SaveChangesAsync();

            var service = new PlayerService(context);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Update(playerDto, 1));
            context.Database.EnsureDeleted();
        }
    }


    [Fact]
    public void GetAll_ReturnsAllPlayers()
    {
        // Arrange
        using (var context = new Context(_options))
        {
            var player1 = new PlayerModel { Name = "John", Username = "john123", Password = "123" };
            var player2 = new PlayerModel { Name = "Mary", Username = "mary456", Password = "123" };
            var match1 = new MatchModel { WinnerId = player1.Id, LoserId = player2.Id };
            var match2 = new MatchModel { WinnerId = player2.Id, LoserId = player1.Id };
            player1.Matches = new List<MatchModel> { match1 };
            player2.Matches = new List<MatchModel> { match2 };
            context.Players.AddRange(new[] { player1, player2 });
            context.SaveChanges();
        }

        // Act
        using (var context = new Context(_options))
        {
            var service = new PlayerService(context);
            var result =  service.GetAll();
            var players = result.Value;

            // Assert
            Assert.Equal(2, players.Count);
            Assert.Contains(players, p => p.Name == "John");
            Assert.Contains(players, p => p.Name == "Mary");
            context.Database.EnsureDeleted();
        }
    }


}

