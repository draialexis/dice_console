using Model.Dice.Faces;
using Model.Dice;
using Model.Games;
using Model.Players;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;

namespace Tests.Model_UTs
{
    public class GameTest
    {

        private static readonly string GAME_NAME = "my game";

        private static readonly Player PLAYER_1 = new("Alice"), PLAYER_2 = new("Bob"), PLAYER_3 = new("Clyde");

        private readonly static NumberDieFace FACE_NUM = new(1);
        private readonly static ImageDieFace FACE_IMG = new(10);
        private readonly static ColorDieFace FACE_CLR = new(1000);

        private readonly static NumberDieFace[] FACES_1 = new NumberDieFace[]
        {
            FACE_NUM,
            new(2),
            new(3),
            new(4)
        };

        private readonly static ImageDieFace[] FACES_2 = new ImageDieFace[]
        {
            FACE_IMG,
            new(20),
            new(30),
            new(40)
        };

        private readonly static ColorDieFace[] FACES_3 = new ColorDieFace[]
        {
            FACE_CLR,
            new(2000),
            new(3000),
            new(4000)
        };

        private static readonly AbstractDie<AbstractDieFace> NUM = new NumberDie(FACES_1), IMG = new ImageDie(FACES_2), CLR = new ColorDie(FACES_3);

        private static readonly IEnumerable<AbstractDie<AbstractDieFace>> DICE =
            new List<AbstractDie<AbstractDieFace>>() { NUM, IMG, CLR }
            .AsEnumerable();

        [Fact]
        public void TestNamePropertyGet()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);

            // Act
            string actual = game.Name;

            // Assert
            Assert.Equal(GAME_NAME, actual);
        }

        [Fact]
        public void TestNamePropertySetWhenValidThenCorrect()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
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
            void action() => game = new(name: name, playerManager: new PlayerManager(), dice: DICE);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetHistory()
        {
            // Arrange
            Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> diceNFaces = new()
            {
                { CLR, FACE_CLR },
                { IMG, FACE_IMG },
                { NUM, FACE_NUM }
            };

            Turn turn1 = Turn.CreateWithDefaultTime(PLAYER_1, diceNFaces);
            Turn turn2 = Turn.CreateWithDefaultTime(PLAYER_2, diceNFaces); // yeah they rolled the same

            IEnumerable<Turn> expected = new List<Turn>() { turn1, turn2 };

            // Act
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE, expected);
            IEnumerable<Turn> actual = game.GetHistory();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDicePropertyGet()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);


            // Act
            IEnumerable<AbstractDie<AbstractDieFace>> actual = game.Dice;
            IEnumerable<AbstractDie<AbstractDieFace>> expected = DICE;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestPerformTurnDoesAddOneTurn()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);

            int n = 5;

            IEnumerable<Player> players = game.GetPlayersFromGame();
            Debug.WriteLine(players);

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
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);

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
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);

            // Act
            void action() => game.GetWhoPlaysNow(); // on an empty collection of players

            // Assert
            Assert.Throws<MemberAccessException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenEmptyThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);

            // Act
            void action() => game.PrepareNextPlayer(PLAYER_1); // on an empty collection of players

            // Assert
            Assert.Throws<MemberAccessException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenNullThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);

            // Act
            void action() => game.PrepareNextPlayer(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenNonExistentThenException()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_2);

            // Act
            void action() => game.PrepareNextPlayer(PLAYER_3);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestPrepareNextPlayerWhenValidThenCorrectWithSeveralPlayers()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);

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
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);

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
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);

            // Act
            Player expected = PLAYER_1;
            Player actual = game.AddPlayerToGame(PLAYER_1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetPlayersFromGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);

            // Act
            Assert.Empty(game.GetPlayersFromGame());
            game.AddPlayerToGame(PLAYER_1);

            // Assert
            Assert.Single(game.GetPlayersFromGame());
        }

        [Fact]
        public void TestUpdatePlayerInGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);

            // Act
            Player expected = PLAYER_2;
            Player actual = game.UpdatePlayerInGame(PLAYER_1, PLAYER_2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRemovePlayerFromGame()
        {
            // Arrange
            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE);
            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);
            game.RemovePlayerFromGame(PLAYER_1);

            // Act
            IEnumerable<Player> expected = new List<Player>() { PLAYER_2 }.AsEnumerable();
            IEnumerable<Player> actual = game.GetPlayersFromGame();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString()
        {
            // Arrange
            DateTime dateTime = DateTime.UtcNow;

            List<Turn> turns = new()
            {
                Turn.CreateWithSpecifiedTime(dateTime, PLAYER_1, new()
                {
                    {NUM, new NumberDieFace(4)},
                    {IMG, new ImageDieFace(40)},
                    {CLR, new ColorDieFace("A00FA0")},
                }),
                Turn.CreateWithSpecifiedTime(dateTime, PLAYER_2, new()
                {
                    {NUM, new NumberDieFace(3)},
                    {IMG, new ImageDieFace(20)},
                    {CLR, new ColorDieFace("A00BB8")},
                }),
            };

            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: DICE, turns: turns);
            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);

            // Act
            string[] dateTimeString = dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Split("T");

            string date = dateTimeString[0];
            string time = dateTimeString[1];

            string expected =
                "Game: my game" +
                "\nPlayers: Alice Bob" +
                "\nNext: Alice" +
                "\nLog:" +
                "\n\t" + date + " " + time + " -- Alice rolled: 4 Assets/images/40.png #A00FA0" +
                "\n\t" + date + " " + time + " -- Bob rolled: 3 Assets/images/20.png #A00BB8" +
                "\n";
            string actual = game.ToString();

            Debug.WriteLine("expected:\n" + expected);
            Debug.WriteLine("actual:\n" + actual);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
