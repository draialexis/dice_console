using Model.Games;
using Model.Dice;
using Model.Dice.Faces;
using Model.Players;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs
{
    public class GameRunnerTest
    {
        private readonly GameRunner stubGameRunner;
        public GameRunnerTest()
        {
            stubGameRunner = new Stub().LoadApp();
        }

        [Fact]
        public void TestConstructorWhenNoGamesThenNewIEnumerable()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act
            expected = new List<Game>().AsEnumerable();
            actual = gameRunner.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestConstructorWhenGamesThenGamesIEnumerable()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager(), stubGameRunner.GetAll().ToList());
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act
            expected = stubGameRunner.GetAll();
            actual = gameRunner.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenGamesThenDoAddAndReturnGames()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            Game game1 = stubGameRunner.GetAll().First();
            Game game2 = stubGameRunner.GetAll().Last();

            // Act
            IEnumerable<Game> expected = new List<Game>() { game1, game2 }.AsEnumerable();
            IEnumerable<Game> actual = new List<Game>()
            {
                gameRunner.Add(game1),
                gameRunner.Add(game2)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenNullThenThrowsException()
        {
            // Arrange


            // Act
            void action() => stubGameRunner.Add(null);// Add() returns the added element if succesful

            // Assert
            Assert.Throws<ArgumentNullException>(action);
            Assert.DoesNotContain(null, stubGameRunner.GetAll());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestGetOneByNameWhenInvalidThenThrowsException(string name)
        {
            // Arrange


            // Act
            void action() => stubGameRunner.GetOneByName(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetOneByNameWhenValidButNotExistThenReturnNull()
        {
            // Arrange


            // Act
            Game result = stubGameRunner.GetOneByName("thereisbasicallynowaythatthisgamenamealreadyexists");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestGetOneByNameWhenValidThenReturnGame()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            Game game = stubGameRunner.GetAll().First();

            // Act
            Game actual = gameRunner.Add(game);
            Game expected = game;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestWhenRemoveExistsThenSucceeds()
        {
            // Arrange
            Game game = new("blargh", new PlayerManager(), stubGameRunner.GetAll().First().Dice);
            stubGameRunner.Add(game);

            // Act
            stubGameRunner.Remove(game);

            // Assert
            Assert.DoesNotContain(game, stubGameRunner.GetAll());
        }

        [Fact]
        public void TestRemoveWhenGivenNullThenThrowsException()
        {
            // Arrange


            // Act
            void action() => stubGameRunner.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestRemoveWhenGiveenNonExistentThenFailsSilently()
        {
            // Arrange
            Game notGame = new("blargh", new PlayerManager(), stubGameRunner.GetAll().First().Dice);
            IEnumerable<Game> expected = stubGameRunner.GetAll();

            // Act
            stubGameRunner.Remove(notGame);
            IEnumerable<Game> actual = stubGameRunner.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestUpdateWhenValidThenSucceeds()
        {
            // Arrange
            string oldName = "blargh";
            string newName = "blargh2.0";
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            Game game = new(oldName, new PlayerManager(), stubGameRunner.GetAll().First().Dice);
            game.PlayerManager.Add(new("Alice"));
            gameRunner.Add(game);

            Game oldGame = gameRunner.GetAll().First();
            Game newGame = new(newName, oldGame.PlayerManager, oldGame.Dice);

            // Act
            int oldSize = gameRunner.GetAll().Count();
            gameRunner.Update(oldGame, newGame);
            int newSize = gameRunner.GetAll().Count();

            // Assert
            Assert.NotEqual(oldName, newName);
            Assert.DoesNotContain(oldGame, gameRunner.GetAll());
            Assert.Contains(newGame, gameRunner.GetAll());
            Assert.Equal(oldSize, newSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateWhenValidBeforeAndInvalidAfterThenDoesNotGo(string badName)
        {
            // Arrange
            int expectedSize = stubGameRunner.GetAll().Count();
            Game oldGame = stubGameRunner.GetAll().First();

            // Act
            void action() => stubGameRunner.Update(oldGame, new(badName, oldGame.PlayerManager, oldGame.Dice));
            int actualSize = stubGameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, stubGameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestUpdateWhenValidBeforeAndNullAfterThenDoesNotGo()
        {
            // Arrange
            int expectedSize = stubGameRunner.GetAll().Count();
            Game oldGame = stubGameRunner.GetAll().First();

            // Act
            void action() => stubGameRunner.Update(oldGame, null);
            int actualSize = stubGameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, stubGameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            int expectedSize = stubGameRunner.GetAll().Count();
            Game oldGame = stubGameRunner.GetAll().First();

            // Act
            void action() => stubGameRunner.Update(null, new("newgamename", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = stubGameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, stubGameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateWhenInvalidBeforeAndValidAfterThenDoesNotGo(string badName)
        {
            // Arrange
            int expectedSize = stubGameRunner.GetAll().Count();
            Game oldGame = stubGameRunner.GetAll().First();

            // Act
            void action() => stubGameRunner.Update(new(badName, oldGame.PlayerManager, oldGame.Dice), new("valid", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = stubGameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, stubGameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }
    }
}
