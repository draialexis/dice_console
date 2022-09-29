using Data;
using Model;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs
{
    public class GameRunnerTest
    {
        private readonly GameRunner stubGameRunner;
        public GameRunnerTest()
        {
            IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> globalDieManager = new DieManager();

            List<AbstractDie<AbstractDieFace>> monopolyDice = new();
            List<AbstractDie<AbstractDieFace>> dndDice = new();

            string monopolyName = "Monopoly";
            string dndName = "DnD";

            NumberDieFace[] d6Faces = new NumberDieFace[] { new(1), new(2), new(3), new(4), new(5), new(6) };

            monopolyDice.Add(new NumberDie(d6Faces));
            monopolyDice.Add(new NumberDie(d6Faces));
            monopolyDice.Add(new ColorDie(new("#ff0000"), new("#00ff00"), new("#0000ff"), new("#ffff00"), new("#000000"), new("#ffffff")));

            NumberDieFace[] d20Faces = new NumberDieFace[] {
                new(1), new(2), new(3), new(4), new(5),
                new(6), new(7), new(8), new(9), new(10),
                new(11), new(12), new(13), new(14), new(15),
                new(16), new(17), new(18), new(19), new(20)
            };

            dndDice.Add(new NumberDie(d20Faces));

            globalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(monopolyName, monopolyDice.AsEnumerable()));
            globalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(dndName, dndDice.AsEnumerable()));

            IEnumerable<AbstractDie<AbstractDieFace>> dice1 = globalDieManager.GetOneByName(monopolyName).Value;
            IEnumerable<AbstractDie<AbstractDieFace>> dice2 = globalDieManager.GetOneByName(dndName).Value;

            string g1 = "game1", g2 = "game2", g3 = "game3";

            Game game1 = new(name: g1, playerManager: new PlayerManager(), dice: dice1);
            Game game2 = new(name: g2, playerManager: new PlayerManager(), dice: dice2);
            Game game3 = new(name: g3, playerManager: new PlayerManager(), dice: dice1);

            List<Game> games = new() { game1, game2, game3 };

            Player player1 = new("Alice"), player2 = new("Bob"), player3 = new("Clyde");

            PlayerManager globalPlayerManager = new();
            globalPlayerManager.Add(player1);
            globalPlayerManager.Add(player2);
            globalPlayerManager.Add(player3);

            GameRunner gameRunner = new(globalPlayerManager, globalDieManager, games);

            game1.PlayerManager.Add(player1);
            game1.PlayerManager.Add(player2);

            game2.PlayerManager.Add(player1);
            game2.PlayerManager.Add(player2);
            game2.PlayerManager.Add(player3);

            game3.PlayerManager.Add(player1);
            game3.PlayerManager.Add(player3);

            foreach (Game game in games)
            {
                for (int i = 0; i < 10; i++)
                {
                    Player currentPlayer = game.GetWhoPlaysNow();
                    game.PerformTurn(currentPlayer);
                    game.PrepareNextPlayer(currentPlayer);
                }
            }

            stubGameRunner = gameRunner;
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
