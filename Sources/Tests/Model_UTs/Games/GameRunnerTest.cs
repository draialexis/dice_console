using Data;
using Model.Dice;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class GameRunnerTest
    {
        private readonly GameRunner stubGameRunner = new Stub().LoadApp();

        [Fact]
        public void TestPlayGameWhenPlayThenAddNewTurnToHistory()
        {
            // Arrange
            GameRunner gameRunner = stubGameRunner;
            Game game = gameRunner.GameManager.GetAll().First();

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
            Assert.DoesNotContain(gameRunner.GameManager.GetOneByName(name), gameRunner.GameManager.GetAll());
            gameRunner.StartNewGame(name, new PlayerManager(), stubGameRunner.GameManager.GetAll().First().Dice);

            // Assert
            Assert.Contains(gameRunner.GameManager.GetOneByName(name), gameRunner.GameManager.GetAll());
        }
    }
}
