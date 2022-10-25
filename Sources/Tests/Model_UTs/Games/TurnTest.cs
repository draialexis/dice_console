using Data;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs.Games
{
    public class TurnTest

    {
        private readonly MasterOfCeremonies stubMasterOfCeremonies = new Stub().LoadApp()?.Result;
        private readonly ReadOnlyDictionary<Die, Face> DICE_N_FACES_1;
        private readonly ReadOnlyDictionary<Die, Face> DICE_N_FACES_2;

        public TurnTest()
        {

            DICE_N_FACES_1 = stubMasterOfCeremonies.GameManager.GetAll()?.Result.First().GetHistory().First().DiceNFaces;
            DICE_N_FACES_2 = stubMasterOfCeremonies.GameManager.GetAll()?.Result.Last().GetHistory().Last().DiceNFaces;
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeNotUTCThenValid()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Local);
            Player player = new("Alice");
            Assert.NotEqual(DateTimeKind.Utc, dateTime.Kind);

            // Act
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES_1);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.When.Kind);
            Assert.Equal(dateTime.ToUniversalTime(), turn.When);
        }



        [Fact]
        public void TestCreateWithSpecifiedTimeUTCThenValid()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            Player player = new("Bobby");
            Assert.Equal(DateTimeKind.Utc, dateTime.Kind);

            // Act
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES_1);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.When.Kind);
            Assert.Equal(dateTime.ToUniversalTime(), turn.When);
        }


        [Fact]
        public void TestCreateWithSpecifiedTimeNullPlayerThenException()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, null, DICE_N_FACES_1);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeNullFacesThenException()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            Player player = new("Chucky");

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, player, null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }


        [Fact]
        public void TestCreateWithSpecifiedTimeEmptyFacesThenException()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            Player player = new("Chucky");

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, player, new Dictionary<Die, Face>());

            // Assert
            Assert.Throws<ArgumentException>(action);
        }



        [Fact]
        public void TestCreateWithDefaultTimeThenValid()
        {
            // Arrange
            Player player = new("Chloe");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player, DICE_N_FACES_1);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.When.Kind);
            Assert.Equal(DateTime.Now.ToUniversalTime().Date, turn.When.Date);
            // N.B.: might fail between 11:59:59PM and 00:00:00AM
        }

        [Fact]
        public void TestDiceNFacesProperty()
        {
            // Arrange
            Player player = new("Erika");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player, DICE_N_FACES_1);
            IEnumerable<KeyValuePair<Die, Face>> expected = DICE_N_FACES_1.AsEnumerable();

            // Assert
            Assert.Equal(expected, turn.DiceNFaces);
        }

        [Fact]
        public void TestEqualsFalseIfNotTurn()
        {
            // Arrange
            Point point;
            Turn turn;
            Player player = new("Freddie");

            // Act
            point = new(1, 2);
            turn = Turn.CreateWithDefaultTime(player, DICE_N_FACES_1);

            // Assert
            Assert.False(point.Equals(turn));
            Assert.False(point.GetHashCode().Equals(turn.GetHashCode()));
            Assert.False(turn.Equals(point));
            Assert.False(turn.GetHashCode().Equals(point.GetHashCode()));
        }

        [Fact]
        public void TestGoesThruToSecondMethodIfObjIsTypeTurn()
        {
            // Arrange
            object t1;
            Turn t2;
            Player player1 = new Player("Marvin");
            Player player2 = new Player("Noah");

            // Act
            t1 = Turn.CreateWithDefaultTime(player1, DICE_N_FACES_1);
            t2 = Turn.CreateWithDefaultTime(player2, DICE_N_FACES_2);

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.False(t2.Equals(t1));
            Assert.False(t2.GetHashCode().Equals(t1.GetHashCode()));
        }


        [Fact]
        public void TestEqualsFalseIfNotSamePlayer()
        {
            // Arrange
            Player player1 = new("Panama");
            Player player2 = new("Clyde");

            // Act
            Turn t1 = Turn.CreateWithDefaultTime(player1, DICE_N_FACES_2);
            Turn t2 = Turn.CreateWithDefaultTime(player2, DICE_N_FACES_2);

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.False(t2.Equals(t1));
            Assert.False(t2.GetHashCode().Equals(t1.GetHashCode()));
        }

        [Fact]
        public void TestEqualsFalseIfNotSameTime()
        {
            // Arrange
            Player player = new("Oscar");

            // Act
            Turn t1 = Turn.CreateWithSpecifiedTime(new DateTime(1994, 07, 10), player, DICE_N_FACES_1);
            Turn t2 = Turn.CreateWithSpecifiedTime(new DateTime(1991, 08, 20), player, DICE_N_FACES_1);

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.False(t2.Equals(t1));
            Assert.False(t2.GetHashCode().Equals(t1.GetHashCode()));
        }

        [Fact]
        public void TestEqualsFalseIfNotSameDiceNFaces()
        {
            // Arrange
            Player player = new("Django");

            // Act
            Turn t1 = Turn.CreateWithDefaultTime(player, DICE_N_FACES_1);
            Turn t2 = Turn.CreateWithDefaultTime(player, DICE_N_FACES_2);

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.False(t2.Equals(t1));
            Assert.False(t2.GetHashCode().Equals(t1.GetHashCode()));
        }

        [Fact]
        public void TestEqualsTrueIfExactlySameProperties()
        {
            // Arrange
            Player player = new("Elyse");

            // Act
            Turn t1 = Turn.CreateWithSpecifiedTime(new DateTime(1990, 04, 29), player, DICE_N_FACES_1);
            Turn t2 = Turn.CreateWithSpecifiedTime(new DateTime(1990, 04, 29), player, DICE_N_FACES_1);

            // Assert
            Assert.True(t1.Equals(t2));
            Assert.True(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.True(t2.Equals(t1));
            Assert.True(t2.GetHashCode().Equals(t1.GetHashCode()));
        }
    }
}
