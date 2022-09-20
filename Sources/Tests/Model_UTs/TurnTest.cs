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

        }

        [Fact]
        public void TestCreateWithSpecifiedTimeUTCThenValid()
        {

        }

        [Fact]
        public void TestCreateWithSpecifiedTimeNullPlayerThenException()
        {

        }

        [Fact]
        public void TestCreateWithDefaultTimeThenValid()
        {
            // check that the date (not the time part) is the same as today
            // ... would fail tests at 11:59:59pm -- acceptable (?)

        }

        [Fact]
        public void TestCreateWithDefaultTimeNullPlayerThenException()
        {

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
