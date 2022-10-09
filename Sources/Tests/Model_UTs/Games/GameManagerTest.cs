using Data;
using Model;
using Model.Dice;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Data_UTs.Games;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class GameManagerTest
    {
        private readonly MasterOfCeremonies stubGameRunner = new Stub().LoadApp().Result;

        [Fact]
        public async Task TestConstructorReturnsEmptyEnumerableAsync()
        {
            // Arrange
            GameManager gm = new();
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act 
            expected = new Collection<Game>();
            actual = await gm.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestAddWhenGamesThenDoAddAndReturnGamesAsync()
        {
            // Arrange
            GameManager gm = new();
            Game game1 = (await stubGameRunner.GameManager.GetAll()).First();
            Game game2 = (await stubGameRunner.GameManager.GetAll()).Last();

            // Act
            IEnumerable<Game> expected = new List<Game>() { game1, game2 }.AsEnumerable();
            IEnumerable<Game> actual = new List<Game>()
            {
                await gm.Add(game1),
                await gm.Add(game2)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestAddWhenNullThenThrowsException()
        {
            // Arrange
            GameManager gm = new();

            // Act
            async Task actionAsync() => await gm.Add(null);// Add() returns the added element if succesful

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
            Assert.DoesNotContain(null, await stubGameRunner.GameManager.GetAll());
        }

        [Fact]
        public void TestGetOneByIdThrowsException()
        {
            // Arrange
            GameManager gm = new();

            // Act

            void action() => gm.GetOneByID(Guid.NewGuid());

            // Assert
            Assert.Throws<NotImplementedException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task TestGetOneByNameWhenInvalidThenThrowsExceptionAsync(string name)
        {
            // Arrange
            GameManager gm = new();

            // Act
            async Task actionAsync() => await gm.GetOneByName(name);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(actionAsync);
        }

        [Fact]
        public async Task TestGetOneByNameWhenValidButNotExistThenReturnNullAsync()
        {
            // Arrange
            GameManager gm = new();

            // Act
            Game result = await gm.GetOneByName("thereisbasicallynowaythatthisgamenamealreadyexists");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetOneByNameWhenValidThenReturnGameAsync()
        {
            // Arrange
            GameManager gm = new();
            Game game = (await stubGameRunner.GameManager.GetAll()).First();

            // Act
            Game actual = await gm.Add(game);
            Game expected = game;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestWhenRemoveExistsThenSucceeds()
        {
            // Arrange
            GameManager gm = new();

            Game game = new("blargh", new PlayerManager(), (await stubGameRunner.GameManager.GetAll()).First().Dice);
            await gm.Add(game);

            // Act
            gm.Remove(game);

            // Assert
            Assert.DoesNotContain(game, await gm.GetAll());
        }

        [Fact]
        public void TestRemoveWhenGivenNullThenThrowsException()
        {
            // Arrange
            GameManager gm = new();

            // Act
            void action() => gm.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TestRemoveWhenGivenNonExistentThenFailsSilentlyAsync()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;

            Game notGame = new("blargh", new PlayerManager(), (await stubGameRunner.GameManager.GetAll()).First().Dice);
            IEnumerable<Game> expected = await stubGameRunner.GameManager.GetAll();

            // Act
            gm.Remove(notGame);
            IEnumerable<Game> actual = await gm.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public async Task TestUpdateWhenValidThenSucceeds()
        {
            // Arrange
            GameManager gm = new();

            string oldName = "blargh";
            string newName = "blargh2.0";
            Game game = new(oldName, new PlayerManager(), (await stubGameRunner.GameManager.GetAll()).First().Dice);
            await game.PlayerManager.Add(new("Alice"));
            await gm.Add(game);

            Game oldGame = (await gm.GetAll()).First();
            Game newGame = new(newName, oldGame.PlayerManager, oldGame.Dice);

            // Act
            int expectedSize = (await gm.GetAll()).Count();
            await gm.Update(oldGame, newGame);
            int actualSize = (await gm.GetAll()).Count();

            // Assert
            Assert.NotEqual(oldName, newName);
            Assert.DoesNotContain(oldGame, await gm.GetAll());
            Assert.Contains(newGame, await gm.GetAll());
            Assert.Equal(expectedSize, actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task TestUpdateWhenValidBeforeAndInvalidAfterThenDoesNotGoAsync(string badName)
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;

            int expectedSize = (await gm.GetAll()).Count();
            Game oldGame = (await gm.GetAll()).First();

            // Act
            void action() => gm.Update(oldGame, new(badName, oldGame.PlayerManager, oldGame.Dice));
            int actualSize = (await gm.GetAll()).Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, await gm.GetAll()); // still there
            Assert.Equal(expectedSize, actualSize);
        }

        [Fact]
        public async Task TestUpdateWhenValidBeforeAndNullAfterThenDoesNotGoAsync()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = (await gm.GetAll()).Count();
            Game oldGame = (await gm.GetAll()).First();

            // Act
            async Task actionAsync() => await gm.Update(oldGame, null);
            int actualSize = (await gm.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync); // thrown by constructor
            Assert.Contains(oldGame, await gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public async Task TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = (await gm.GetAll()).Count();
            Game oldGame = (await gm.GetAll()).First();

            // Act
            async Task actionAsync() => await gm.Update(null, new("newgamename", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = (await gm.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync); // thrown by constructor
            Assert.Contains(oldGame, await gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task TestUpdateWhenInvalidBeforeAndValidAfterThenDoesNotGoAsync(string badName)
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = (await gm.GetAll()).Count();
            Game oldGame = (await gm.GetAll()).First();

            // Act
            void action() => gm.Update(new(badName, oldGame.PlayerManager, oldGame.Dice), new("valid", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = (await gm.GetAll()).Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, await gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

    }
}
