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
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class GameManagerTest
    {
        private readonly MasterOfCeremonies stubGameRunner = new Stub().LoadApp();

        [Fact]
        public void TestConstructorReturnsEmptyEnumerable()
        {
            // Arrange
            GameManager gm = new();
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act
            expected = new Collection<Game>();
            actual = gm.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenGamesThenDoAddAndReturnGames()
        {
            // Arrange
            GameManager gm = new();
            Game game1 = stubGameRunner.GameManager.GetAll().First();
            Game game2 = stubGameRunner.GameManager.GetAll().Last();

            // Act
            IEnumerable<Game> expected = new List<Game>() { game1, game2 }.AsEnumerable();
            IEnumerable<Game> actual = new List<Game>()
            {
                gm.Add(game1),
                gm.Add(game2)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenNullThenThrowsException()
        {
            // Arrange
            GameManager gm = new();

            // Act
            void action() => gm.Add(null);// Add() returns the added element if succesful

            // Assert
            Assert.Throws<ArgumentNullException>(action);
            Assert.DoesNotContain(null, stubGameRunner.GameManager.GetAll());
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
        public void TestGetOneByNameWhenInvalidThenThrowsException(string name)
        {
            // Arrange
            GameManager gm = new();

            // Act
            void action() => gm.GetOneByName(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetOneByNameWhenValidButNotExistThenReturnNull()
        {
            // Arrange
            GameManager gm = new();

            // Act
            Game result = gm.GetOneByName("thereisbasicallynowaythatthisgamenamealreadyexists");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestGetOneByNameWhenValidThenReturnGame()
        {
            // Arrange
            GameManager gm = new();
            Game game = stubGameRunner.GameManager.GetAll().First();

            // Act
            Game actual = gm.Add(game);
            Game expected = game;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestWhenRemoveExistsThenSucceeds()
        {
            // Arrange
            GameManager gm = new();

            Game game = new("blargh", new PlayerManager(), stubGameRunner.GameManager.GetAll().First().Dice);
            gm.Add(game);

            // Act
            gm.Remove(game);

            // Assert
            Assert.DoesNotContain(game, gm.GetAll());
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
        public void TestRemoveWhenGivenNonExistentThenFailsSilently()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;

            Game notGame = new("blargh", new PlayerManager(), stubGameRunner.GameManager.GetAll().First().Dice);
            IEnumerable<Game> expected = stubGameRunner.GameManager.GetAll();

            // Act
            gm.Remove(notGame);
            IEnumerable<Game> actual = gm.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestUpdateWhenValidThenSucceeds()
        {
            // Arrange
            GameManager gm = new();

            string oldName = "blargh";
            string newName = "blargh2.0";
            Game game = new(oldName, new PlayerManager(), stubGameRunner.GameManager.GetAll().First().Dice);
            game.PlayerManager.Add(new("Alice"));
            gm.Add(game);

            Game oldGame = gm.GetAll().First();
            Game newGame = new(newName, oldGame.PlayerManager, oldGame.Dice);

            // Act
            int expectedSize = gm.GetAll().Count();
            gm.Update(oldGame, newGame);
            int actualSize = gm.GetAll().Count();

            // Assert
            Assert.NotEqual(oldName, newName);
            Assert.DoesNotContain(oldGame, gm.GetAll());
            Assert.Contains(newGame, gm.GetAll());
            Assert.Equal(expectedSize, actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateWhenValidBeforeAndInvalidAfterThenDoesNotGo(string badName)
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;

            int expectedSize = gm.GetAll().Count();
            Game oldGame = gm.GetAll().First();

            // Act
            void action() => gm.Update(oldGame, new(badName, oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gm.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, gm.GetAll()); // still there
            Assert.Equal(expectedSize, actualSize);
        }

        [Fact]
        public void TestUpdateWhenValidBeforeAndNullAfterThenDoesNotGo()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = gm.GetAll().Count();
            Game oldGame = gm.GetAll().First();

            // Act
            void action() => gm.Update(oldGame, null);
            int actualSize = gm.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = gm.GetAll().Count();
            Game oldGame = gm.GetAll().First();

            // Act
            void action() => gm.Update(null, new("newgamename", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gm.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateWhenInvalidBeforeAndValidAfterThenDoesNotGo(string badName)
        {
            // Arrange
            IManager<Game> gm = stubGameRunner.GameManager;
            int expectedSize = gm.GetAll().Count();
            Game oldGame = gm.GetAll().First();

            // Act
            void action() => gm.Update(new(badName, oldGame.PlayerManager, oldGame.Dice), new("valid", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gm.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, gm.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

    }
}
