using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs
{
    public class TurnTest

    {
        private readonly Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> DICE_N_FACES;
        private static readonly AbstractDieFace FACE_ONE = new NumberDieFace(1);
        private static readonly AbstractDieFace FACE_TWO = new NumberDieFace(12);
        private static readonly AbstractDieFace FACE_THREE = new ImageDieFace(54);
        private static readonly AbstractDieFace FACE_FOUR = new ColorDieFace(16548);

        private readonly static NumberDieFace[] FACES1 = new NumberDieFace[]
        {
            FACE_ONE as NumberDieFace,
            new NumberDieFace(2),
            new NumberDieFace(3),
            new NumberDieFace(4)
        };

        private readonly static NumberDieFace[] FACES2 = new NumberDieFace[] {
                  new NumberDieFace(9),
                  new NumberDieFace(10),
                  new NumberDieFace(11),
                  FACE_TWO as NumberDieFace,
                  new NumberDieFace(13),
                  new NumberDieFace(14)
            };

        private readonly static ImageDieFace[] FACES3 = new ImageDieFace[] {
                  new ImageDieFace(13),
                  new ImageDieFace(27),
                  new ImageDieFace(38),
                  FACE_THREE as ImageDieFace
            };

        private readonly static ColorDieFace[] FACES4 = new ColorDieFace[] {
                  new(11651),
                  new(24651),
                  FACE_FOUR as ColorDieFace,
                  new(412)
            };

        private readonly AbstractDie<AbstractDieFace> NUM1 = new NumberDie(FACES1);
        private readonly AbstractDie<AbstractDieFace> NUM2 = new NumberDie(FACES2);
        private readonly AbstractDie<AbstractDieFace> IMG1 = new ImageDie(FACES3);
        private readonly AbstractDie<AbstractDieFace> CLR1 = new ColorDie(FACES4);

        public TurnTest()
        {
            DICE_N_FACES = new()
            {
                { NUM1, FACE_ONE },
                { NUM2, FACE_TWO },
                { IMG1, FACE_THREE },
                { CLR1, FACE_FOUR }
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
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES);

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
            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES);

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
            void action() => Turn.CreateWithSpecifiedTime(dateTime, null, DICE_N_FACES);

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
            DICE_N_FACES.Clear();

            // Act
            void action() => Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }



        [Fact]
        public void TestCreateWithDefaultTimeThenValid()
        {
            // Arrange
            Player player = new("Chloe");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player, DICE_N_FACES);

            // Assert
            Assert.Equal(DateTimeKind.Utc, turn.When.Kind);
            Assert.Equal(DateTime.Now.ToUniversalTime().Date, turn.When.Date);
            // N.B.: might fail between 11:59:59PM and 00:00:00AM
        }



        [Fact]
        public void TestToStringValidIfAllNormal()
        {
            // Arrange
            DateTime dateTime = new(year: 2018, month: 06, day: 15, hour: 16, minute: 30, second: 0, kind: DateTimeKind.Utc);
            string name = "Bobby";
            Player player = new(name);
            string expected = $"2018-06-15 16:30:00 -- {name} rolled: "
                + FACE_ONE.ToString() + " "
                + FACE_TWO.ToString() + " "
                + FACE_THREE.ToString() + " "
                + FACE_FOUR.ToString();

            Turn turn = Turn.CreateWithSpecifiedTime(dateTime, player, DICE_N_FACES);

            // Act
            string actual = turn.ToString();
            Debug.WriteLine(actual);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDiceNFacesProperty()
        {
            // Arrange
            Player player = new("Erika");

            // Act
            Turn turn = Turn.CreateWithDefaultTime(player, DICE_N_FACES);
            IEnumerable<KeyValuePair<AbstractDie<AbstractDieFace>, AbstractDieFace>> expected = DICE_N_FACES.AsEnumerable();

            // Assert
            Assert.Equal(expected, turn.DiceNFaces);
        }
    }
}
