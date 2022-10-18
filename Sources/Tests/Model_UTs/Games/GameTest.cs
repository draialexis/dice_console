using Data;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class GameTest
    {
        private readonly MasterOfCeremonies stubMasterOfCeremonies = new Stub().LoadApp()?.Result;
        private static readonly string GAME_NAME = "my game";

        private static readonly Player PLAYER_1 = new("Alice"), PLAYER_2 = new("Bob"), PLAYER_3 = new("Clyde");
        private readonly IEnumerable<Die> DICE_1, DICE_2;
        public GameTest()
        {
            IEnumerable<DiceGroup> diceGroups = stubMasterOfCeremonies.DiceGroupManager.GetAll()?.Result;
            DICE_1 = diceGroups.First().Dice;
            DICE_2 = diceGroups.Last().Dice;
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
        public async Task TestGetHistory()
        {
            // Arrange
            IEnumerable<KeyValuePair<Die, Face>> diceNFaces =
                (await stubMasterOfCeremonies.GameManager.GetAll())
                .First()
                .GetHistory()
                .First().DiceNFaces;

            Turn turn1 = Turn.CreateWithSpecifiedTime(new(1, 2, 3), PLAYER_1, diceNFaces);
            Turn turn2 = Turn.CreateWithSpecifiedTime(new(1, 2, 3), PLAYER_2, diceNFaces); // yeah they rolled the same

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
        public async Task TestPerformTurnDoesAddOneTurnAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            await game.PlayerManager.Add(PLAYER_1);
            await game.PlayerManager.Add(PLAYER_2);

            int n = 5;

            Player currentPlayer;
            for (int i = 0; i < n; i++)
            {
                currentPlayer = await game.GetWhoPlaysNow();
                game.PerformTurn(currentPlayer);
                await game.PrepareNextPlayer(currentPlayer);
            }

            // Act
            int actual = game.GetHistory().Count;
            int expected = n;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestGetWhoPlaysNowWhenValidThenCorrectAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            await game.PlayerManager.Add(PLAYER_1);
            await game.PlayerManager.Add(PLAYER_2);

            // Act
            Player actual = await game.GetWhoPlaysNow();
            Player expected = PLAYER_1;

            await game.PrepareNextPlayer(actual);

            Player actual2 = await game.GetWhoPlaysNow();
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
            async Task actionAsync() => await game.GetWhoPlaysNow(); // on an empty collection of players

            // Assert
            Assert.ThrowsAsync<MemberAccessException>(actionAsync);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenEmptyThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);
            // Act
            async Task actionAsync() => await game.PrepareNextPlayer(PLAYER_1); // on an empty collection of players

            // Assert
            Assert.ThrowsAsync<MemberAccessException>(actionAsync);
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
            async Task actionAsync() => await game.PrepareNextPlayer(null);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
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
            async Task actionAsync() => await game.PrepareNextPlayer(PLAYER_3);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(actionAsync);
        }

        [Fact]
        public async Task TestPrepareNextPlayerWhenValidThenCorrectWithSeveralPlayersAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            await game.PlayerManager.Add(PLAYER_1);
            await game.PlayerManager.Add(PLAYER_2);

            // Act
            Player expected = PLAYER_2;

            Assert.Equal(PLAYER_1, await game.GetWhoPlaysNow());
            await game.PrepareNextPlayer(PLAYER_1);

            Player actual = await game.GetWhoPlaysNow();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestPrepareNextPlayerWhenValidThenCorrectWithOnePlayerAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            await game.PlayerManager.Add(PLAYER_1);

            // Act
            Player expected = PLAYER_1;

            Assert.Equal(PLAYER_1, await game.GetWhoPlaysNow());
            await game.PrepareNextPlayer(PLAYER_1);

            Player actual = await game.GetWhoPlaysNow();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestAddPlayerToGameAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            // Act
            Player expected = PLAYER_1;
            Player actual = await game.PlayerManager.Add(PLAYER_1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestGetPlayersFromGameAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            // Act
            Assert.Empty(await game.PlayerManager.GetAll());
            await game.PlayerManager.Add(PLAYER_1);

            // Assert
            Assert.Single(await game.PlayerManager.GetAll());
        }

        [Fact]
        public async Task TestUpdatePlayerInGameAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_2);

            await game.PlayerManager.Add(PLAYER_1);

            // Act
            Player expected = PLAYER_2;
            Player actual = await game.PlayerManager.Update(PLAYER_1, PLAYER_2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestRemovePlayerFromGameAsync()
        {
            // Arrange
            Game game = new(name: GAME_NAME,
                playerManager: new PlayerManager(),
                dice: DICE_1);

            await game.PlayerManager.Add(PLAYER_1);
            await game.PlayerManager.Add(PLAYER_2);
            game.PlayerManager.Remove(PLAYER_1);

            // Act
            IEnumerable<Player> expected = new List<Player>() { PLAYER_2 }.AsEnumerable();
            IEnumerable<Player> actual = await game.PlayerManager.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
