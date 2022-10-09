using Data;
using Model.Dice;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class MasterOfCeremoniesTest
    {
        private readonly MasterOfCeremonies stubMasterOfCeremonies = new Stub().LoadApp().Result;

        [Fact]
        public async Task TestPlayGameWhenPlayThenAddNewTurnToHistoryAsync()
        {
            // Arrange
            MasterOfCeremonies masterOfCeremonies = stubMasterOfCeremonies;
            Game game = (await masterOfCeremonies.GameManager.GetAll()).First();

            // Act
            int turnsBefore = game.GetHistory().Count();
            await MasterOfCeremonies.PlayGame(game);
            int turnsAfter = game.GetHistory().Count();

            // Assert
            Assert.Equal(turnsBefore + 1, turnsAfter);
        }

        [Fact]
        public async Task TestStartNewGame()
        {
            // Arrange
            MasterOfCeremonies masterOfCeremonies = stubMasterOfCeremonies;
            string name = "blargh";

            // Act
            Assert.DoesNotContain(
                await masterOfCeremonies.GameManager.GetOneByName(name),
                await masterOfCeremonies.GameManager.GetAll()
                );

            await masterOfCeremonies.StartNewGame(
                name,
                new PlayerManager(),
                stubMasterOfCeremonies.GameManager.GetAll().Result.First().Dice
                );

            // Assert
            Assert.Contains(
                await masterOfCeremonies.GameManager.GetOneByName(name),
                await masterOfCeremonies.GameManager.GetAll()
                );
        }
    }
}
