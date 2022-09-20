using Model;
using System;
using Xunit;

namespace Tests.Model_UTs
{
    public class TurnTest
    {
        [Fact]
        public void TestCreateWithSpecifiedTimeNotUTCThenValid()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Local);
            Player player = new("Alice");
            Assert.NotEqual(DateTimeKind.Utc, dateTime.Kind);

            // Act
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.when.Kind);
            Assert.Equal(dateTime.ToUniversalTime(), turn.when);
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeUTCThenValid()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            Player player = new("Bobby");
            Assert.Equal(DateTimeKind.Utc, dateTime.Kind);

            // Act
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.when.Kind);
            Assert.Equal(dateTime.ToUniversalTime(), turn.when);
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeNullPlayerThenException()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestCreateWithDefaultTimeThenValid()
        {
            // Arrange
            Player player = new("Chloe");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.when.Kind);
            Assert.Equal(DateTime.Now.ToUniversalTime().Date, turn.when.Date);
            /*** N.B.: might fail between 11:59:59PM and 00:00:00AM ***/
        }

        [Fact]
        public void TestCreateWithDefaultTimeNullPlayerThenException()
        {
            // Arrange
            Player player = new("Devon");

            // Act
            static void action() => Turn.CreateWithDefaultTime(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestToStringValidIfAllNormal()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            string name = "Bobby";
            Player player = new(name);
            string expected = $"2018-06-15T16:30:00Z -- {name} rolled <face>, <face>, <face>...";
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player);

            // Act
            string actual = turn.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
