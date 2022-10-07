using Data;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class GameTest
    {
        private readonly MasterOfCeremonies stubMasterOfCeremonies = new Stub().LoadApp();
        private static readonly string GAME_NAME = "my game";

        private static readonly Player PLAYER_1 = new("Alice"), PLAYER_2 = new("Bob"), PLAYER_3 = new("Clyde");
        private readonly IEnumerable<Die> DICE_1, DICE_2;
        public GameTest()
        {
            DICE_1 = stubMasterOfCeremonies.DieGroupManager.GetAll().First().Value;
            DICE_2 = stubMasterOfCeremonies.DieGroupManager.GetAll().Last().Value;
        }



        [Fact]
        public void TestNamePropertyGet()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            // Act
            string actual = game.Name;

            // Assert
            Assert.Equal(GAME_NAME, actual);
        }

        [Fact]
        public void TestNamePropertySetWhenValidThenCorrect()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            string expected = "shitty marmot";

            // Act
            game.Name = expected;
            string actual = game.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestNamePropertySetWhenInvalidThenException(string name)
        {
            // Arrange
            Game game;

            // Act
            void action() => game = new(name: name,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetHistory()
        {
            // Arrange
            Dictionary<Die, Face> diceNFaces = (Dictionary<Die, Face>)stubMasterOfCeremonies.GameManager.GetAll().First().GetHistory().First().DiceNFaces;

            Turn turn1 = Turn.CreateWithDefaultTime(PLAYER_1, diceNFaces);
            Turn turn2 = Turn.CreateWithDefaultTime(PLAYER_2, diceNFaces); // yeah they rolled the same

            IEnumerable<Turn> expected = new List<Turn>() { turn1, turn2 };

            // Act
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1,
                expected);

            IEnumerable<Turn> actual = game.GetHistory();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDicePropertyGet()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);


            // Act
            IEnumerable<Die> actual = game.Dice;
            IEnumerable<Die> expected = DICE_2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestPerformTurnDoesAddOneTurn()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            game.PlayerManager.Add(PLAYER_1);
            game.PlayerManager.Add(PLAYER_2);

            int n = 5;

            Player currentPlayer;
            for (int i = 0; i < n; i++)
            {
                currentPlayer = game.GetWhoPlaysNow();
                game.PerformTurn(currentPlayer);
                game.PrepareNextPlayer(currentPlayer);
            }

            Debug.WriteLine(game);

            // Act
            int actual = game.GetHistory().Count();
            int expected = n;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetWhoPlaysNowWhenValidThenCorrect()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            game.PlayerManager.Add(PLAYER_1);
            game.PlayerManager.Add(PLAYER_2);

            // Act
            Player actual = game.GetWhoPlaysNow();
            Player expected = PLAYER_1;

            game.PrepareNextPlayer(actual);

            Player actual2 = game.GetWhoPlaysNow();
            Player expected2 = PLAYER_2;

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void TestGetWhoPlaysNowWhenInvalidThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            // Act
            void action() => game.GetWhoPlaysNow(); // on an empty collection of players

            // Assert
            Assert.Throws<MemberAccessException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenEmptyThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);
            // Act
            void action() => game.PrepareNextPlayer(PLAYER_1); // on an empty collection of players

            // Assert
            Assert.Throws<MemberAccessException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenNullThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            game.PlayerManager.Add(PLAYER_1);

            // Act
            void action() => game.PrepareNextPlayer(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenNonExistentThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            game.PlayerManager.Add(PLAYER_2);

            // Act
            void action() => game.PrepareNextPlayer(PLAYER_3);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenValidThenCorrectWithSeveralPlayers()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            game.PlayerManager.Add(PLAYER_1);
            game.PlayerManager.Add(PLAYER_2);

            // Act
            Player expected = PLAYER_2;

            Assert.Equal(PLAYER_1, game.GetWhoPlaysNow());
            game.PrepareNextPlayer(PLAYER_1);

            Player actual = game.GetWhoPlaysNow();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenValidThenCorrectWithOnePlayer()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            game.PlayerManager.Add(PLAYER_1);

            // Act
            Player expected = PLAYER_1;

            Assert.Equal(PLAYER_1, game.GetWhoPlaysNow());
            game.PrepareNextPlayer(PLAYER_1);

            Player actual = game.GetWhoPlaysNow();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddPlayerToGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            // Act
            Player expected = PLAYER_1;
            Player actual = game.PlayerManager.Add(PLAYER_1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetPlayersFromGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            // Act
            Assert.Empty(game.PlayerManager.GetAll());
            game.PlayerManager.Add(PLAYER_1);

            // Assert
            Assert.Single(game.PlayerManager.GetAll());
        }

        [Fact]
        public void TestUpdatePlayerInGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            game.PlayerManager.Add(PLAYER_1);

            // Act
            Player expected = PLAYER_2;
            Player actual = game.PlayerManager.Update(PLAYER_1, PLAYER_2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRemovePlayerFromGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            game.PlayerManager.Add(PLAYER_1);
            game.PlayerManager.Add(PLAYER_2);
            game.PlayerManager.Remove(PLAYER_1);

            // Act
            IEnumerable<Player> expected = new List<Player>() { PLAYER_2 }.AsEnumerable();
            IEnumerable<Player> actual = game.PlayerManager.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
