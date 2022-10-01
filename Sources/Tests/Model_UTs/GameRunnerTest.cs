using Model;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Data;

namespace Tests.Model_UTs
{
    public class GameRunnerTest
    {
        private readonly GameRunner stubGameRunner = new Stub().LoadApp();

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
            GameRunner gameRunner = stubGameRunner;

            // Act
            void action() => gameRunner.Add(null);// Add() returns the added element if succesful

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
            GameRunner gameRunner = stubGameRunner;

            // Act
            void action() => gameRunner.GetOneByName(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetOneByNameWhenValidButNotExistThenReturnNull()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;

            // Act
            Game result = gameRunner.GetOneByName("thereisbasicallynowaythatthisgamenamealreadyexists");

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
            GameRunner gameRunner = stubGameRunner;
            Game game = new("blargh", new PlayerManager(), gameRunner.GetAll().First().Dice);
            gameRunner.Add(game);

            // Act
            gameRunner.Remove(game);

            // Assert
            Assert.DoesNotContain(game, gameRunner.GetAll());
        }

        [Fact]
        public void TestRemoveWhenGivenNullThenThrowsException()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;

            // Act
            void action() => gameRunner.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestRemoveWhenGiveenNonExistentThenFailsSilently()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            Game notGame = new("blargh", new PlayerManager(), gameRunner.GetAll().First().Dice);
            IEnumerable<Game> expected = gameRunner.GetAll();

            // Act
            gameRunner.Remove(notGame);
            IEnumerable<Game> actual = gameRunner.GetAll();

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
            GameRunner gameRunner = stubGameRunner;
            int expectedSize = gameRunner.GetAll().Count();
            Game oldGame = gameRunner.GetAll().First();

            // Act
            void action() => gameRunner.Update(oldGame, new(badName, oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, gameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestUpdateWhenValidBeforeAndNullAfterThenDoesNotGo()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            int expectedSize = gameRunner.GetAll().Count();
            Game oldGame = gameRunner.GetAll().First();

            // Act
            void action() => gameRunner.Update(oldGame, null);
            int actualSize = gameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, gameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            int expectedSize = gameRunner.GetAll().Count();
            Game oldGame = gameRunner.GetAll().First();

            // Act
            void action() => gameRunner.Update(null, new("newgamename", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by constructor
            Assert.Contains(oldGame, gameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateWhenInvalidBeforeAndValidAfterThenDoesNotGo(string badName)
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            int expectedSize = gameRunner.GetAll().Count();
            Game oldGame = gameRunner.GetAll().First();

            // Act
            void action() => gameRunner.Update(new(badName, oldGame.PlayerManager, oldGame.Dice), new("valid", oldGame.PlayerManager, oldGame.Dice));
            int actualSize = gameRunner.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by constructor
            Assert.Contains(oldGame, gameRunner.GetAll()); // still there
            Assert.True(expectedSize == actualSize);
        }

        [Fact]
        public void TestPlayGameWhenPlayThenAddNewTurnToHistory()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            Game game = gameRunner.GetAll().First();

            // Act
            int turnsBefore = game.GetHistory().Count();
            GameRunner.PlayGame(game);
            int turnsAfter = game.GetHistory().Count();

            // Assert
            Assert.Equal(turnsBefore + 1, turnsAfter);
        }

        [Fact]
        public void TestStartNewGame()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            string name = "blargh";

            // Act
            Assert.DoesNotContain(gameRunner.GetOneByName(name), gameRunner.GetAll());
            gameRunner.StartNewGame(name, new PlayerManager(), stubGameRunner.GetAll().First().Dice);

            // Assert
            Assert.Contains(gameRunner.GetOneByName(name), gameRunner.GetAll());
        }
    }
}
