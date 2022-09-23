using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace Tests.Model_UTs
{
    public class TurnTest

    {
        private readonly List<AbstractDieFace> FACES;
        private readonly int FACE_ONE = 1;
        private readonly int FACE_TWO = 12;
        private readonly int FACE_THREE = 54;
        private readonly int FACE_FOUR = 16548;

        public TurnTest()
        {
            FACES = new List<AbstractDieFace>
            {
                new NumberDieFace(FACE_ONE),
                new NumberDieFace(FACE_TWO),
                new ImageDieFace(FACE_THREE),
                new ColorDieFace(FACE_FOUR)
            };
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeNotUTCThenValid()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Local);
            Player player = new("Alice");
            Assert.NotEqual(DateTimeKind.Utc, dateTime.Kind);

            // Act
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, FACES);

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
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, FACES);

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
            void action() => Turn.CreateWithSpecifiedTime(dateTime, null, FACES);

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
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestCreateWithSpecifiedTimeEmptyFacesThenException()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            Player player = new("Chucky");
            FACES.Clear();

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, player, FACES);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestCreateWithDefaultTimeThenValid()
        {
            // Arrange
            Player player = new("Chloe");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player, FACES);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.when.Kind);
            Assert.Equal(DateTime.Now.ToUniversalTime().Date, turn.when.Date);
            /*** N.B.: might fail between 11:59:59PM and 00:00:00AM ***/
        }

        [Fact]
        public void TestToStringValidIfAllNormal()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            string name = "Bobby";
            Player player = new(name);
            string expected = $"2018-06-15 16:30:00 -- {name} rolled: "
                + FACE_ONE + " "
                + FACE_TWO
                + " Assets/images/" + FACE_THREE + " "
                + FACE_FOUR.ToString("X6").Insert(0, "#");

            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, FACES);

            // Act
            string actual = turn.ToString();
            Debug.WriteLine(actual);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
