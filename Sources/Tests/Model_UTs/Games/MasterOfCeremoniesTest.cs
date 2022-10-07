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
    public class MasterOfCeremoniesTest
    {
        private readonly MasterOfCeremonies stubMasterOfCeremonies = new Stub().LoadApp();

        [Fact]
        public void TestPlayGameWhenPlayThenAddNewTurnToHistory()
        {
            // Arrange
            MasterOfCeremonies masterOfCeremonies = stubMasterOfCeremonies;
            Game game = masterOfCeremonies.GameManager.GetAll().First();

            // Act
            int turnsBefore = game.GetHistory().Count();
            MasterOfCeremonies.PlayGame(game);
            int turnsAfter = game.GetHistory().Count();

            // Assert
            Assert.Equal(turnsBefore + 1, turnsAfter);
        }

        [Fact]
        public void TestStartNewGame()
        {
            // Arrange
            MasterOfCeremonies masterOfCeremonies = stubMasterOfCeremonies;
            string name = "blargh";

            // Act
            Assert.DoesNotContain(masterOfCeremonies.GameManager.GetOneByName(name), masterOfCeremonies.GameManager.GetAll());
            masterOfCeremonies.StartNewGame(name, new PlayerManager(), stubMasterOfCeremonies.GameManager.GetAll().First().Dice);

            // Assert
            Assert.Contains(masterOfCeremonies.GameManager.GetOneByName(name), masterOfCeremonies.GameManager.GetAll());
        }
    }
}
