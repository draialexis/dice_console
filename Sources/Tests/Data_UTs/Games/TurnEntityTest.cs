using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Games;
using Data.EF.Joins;
using Data.EF.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Tests.Data_UTs.Games
{
    public class TurnEntityTest
    {

        private readonly DieEntity numDie;
        private readonly FaceEntity numFace1 = new NumberFaceEntity() { ID = Guid.NewGuid(), Value = 7 };
        private readonly FaceEntity numFace2 = new NumberFaceEntity() { ID = Guid.NewGuid(), Value = 8 };

        private readonly DieEntity clrDie;
        private readonly FaceEntity clrFace1 = new ColorFaceEntity() { ID = Guid.NewGuid(), A = 255, R = 255, G = 255, B = 255 };
        private readonly FaceEntity clrFace2 = new ColorFaceEntity() { ID = Guid.NewGuid() };

        private readonly DieEntity imgDie;
        private readonly FaceEntity imgFace1 = new ImageFaceEntity() { ID = Guid.NewGuid(), Value = "https://a" };
        private readonly FaceEntity imgFace2 = new ImageFaceEntity() { ID = Guid.NewGuid(), Value = "https://b" };

        private readonly DieTurn dieTurn1;
        private readonly DieTurn dieTurn2;
        private readonly DieTurn dieTurn3;
        private readonly DieTurn dieTurn4;
        private readonly DieTurn dieTurn5;

        private readonly FaceTurn faceTurn1;
        private readonly FaceTurn faceTurn2;
        private readonly FaceTurn faceTurn3;
        private readonly FaceTurn faceTurn4;
        private readonly FaceTurn faceTurn5;

        private readonly TurnEntity turn1;
        private readonly TurnEntity turn2;
        private readonly TurnEntity turn3;

        public TurnEntityTest()
        {
            numDie = new NumberDieEntity() { ID = Guid.NewGuid(), Faces = new List<NumberFaceEntity>() { numFace1 as NumberFaceEntity, numFace2 as NumberFaceEntity } };
            (numFace1 as NumberFaceEntity).NumberDieEntity = (NumberDieEntity)numDie;
            (numFace2 as NumberFaceEntity).NumberDieEntity = (NumberDieEntity)numDie;

            (clrFace2 as ColorFaceEntity).SetValue(Color.FromName("blue"));

            clrDie = new ColorDieEntity() { ID = Guid.NewGuid(), Faces = new List<ColorFaceEntity>() { clrFace1 as ColorFaceEntity, clrFace2 as ColorFaceEntity } };
            (clrFace1 as ColorFaceEntity).ColorDieEntity = (ColorDieEntity)clrDie;
            (clrFace2 as ColorFaceEntity).ColorDieEntity = (ColorDieEntity)clrDie;

            imgDie = new ImageDieEntity() { ID = Guid.NewGuid(), Faces = new List<ImageFaceEntity>() { imgFace1 as ImageFaceEntity, imgFace2 as ImageFaceEntity } };
            (imgFace1 as ImageFaceEntity).ImageDieEntity = (ImageDieEntity)imgDie;
            (imgFace2 as ImageFaceEntity).ImageDieEntity = (ImageDieEntity)imgDie;

            turn1 = new()
            {
                ID = Guid.NewGuid(),
                When = datetime1,
                PlayerEntity = player1,
                PlayerEntityID = player1.ID,
                Dice = new List<DieEntity>
                {
                    numDie,
                    clrDie,
                    imgDie
                },
                Faces = new List<FaceEntity>
                {
                    numFace1,
                    clrFace2,
                    imgFace2
                },

            };

            dieTurn1 = new() { DieEntityID = numDie.ID, DieEntity = numDie, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            dieTurn2 = new() { DieEntityID = clrDie.ID, DieEntity = clrDie, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            dieTurn3 = new() { DieEntityID = imgDie.ID, DieEntity = imgDie, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            turn1.DieTurns = new() { dieTurn1, dieTurn2, dieTurn3 };
            numDie.DieTurns = new() { dieTurn1 };
            clrDie.DieTurns = new() { dieTurn2 };
            imgDie.DieTurns = new() { dieTurn3 };

            faceTurn1 = new() { FaceEntityID = numFace1.ID, FaceEntity = numFace1, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            faceTurn2 = new() { FaceEntityID = clrFace2.ID, FaceEntity = clrFace2, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            faceTurn3 = new() { FaceEntityID = imgFace2.ID, FaceEntity = imgFace2, TurnEntityID = turn1.ID, TurnEntity = turn1 };
            turn1.FaceTurns = new() { faceTurn1, faceTurn2, faceTurn3 };
            numFace1.FaceTurns = new() { faceTurn1 };
            clrFace2.FaceTurns = new() { faceTurn2 };
            imgFace2.FaceTurns = new() { faceTurn3 };

            Guid turn2ID = Guid.NewGuid();

            turn2 = new()
            {
                ID = turn2ID,
                When = datetime2,
                PlayerEntity = player2,
                PlayerEntityID = player2.ID,
                Dice = new List<DieEntity>
                {
                    numDie,
                    clrDie
                },
                Faces = new List<FaceEntity>
                {
                    numFace2,
                    clrFace2
                },

            };

            dieTurn4 = new() { DieEntityID = numDie.ID, DieEntity = numDie, TurnEntityID = turn2.ID, TurnEntity = turn2 };
            dieTurn5 = new() { DieEntityID = clrDie.ID, DieEntity = clrDie, TurnEntityID = turn2.ID, TurnEntity = turn2 };
            turn2.DieTurns = new() { dieTurn4, dieTurn5 };
            numDie.DieTurns = new() { dieTurn4 };
            clrDie.DieTurns = new() { dieTurn5 };

            faceTurn4 = new() { FaceEntityID = numFace2.ID, FaceEntity = numFace2, TurnEntityID = turn2.ID, TurnEntity = turn2 };
            faceTurn5 = new() { FaceEntityID = clrFace2.ID, FaceEntity = clrFace2, TurnEntityID = turn2.ID, TurnEntity = turn2 };
            turn2.FaceTurns = new() { faceTurn4, faceTurn5 };
            numFace2.FaceTurns = new() { faceTurn4 };
            clrFace2.FaceTurns = new() { faceTurn5 };

            turn3 = new()
            {
                ID = turn2ID,
                When = datetime2,
                PlayerEntity = player2,
                PlayerEntityID = player2.ID,
                Dice = new List<DieEntity>
                {
                    numDie,
                    clrDie
                },
                Faces = new List<FaceEntity>
                {
                    numFace2,
                    clrFace2
                },

            };
        }



        private readonly PlayerEntity player1 = new() { ID = Guid.NewGuid(), Name = "Marvin" };
        private readonly PlayerEntity player2 = new() { ID = Guid.NewGuid(), Name = "Barbara" };

        private readonly DateTime datetime1 = new(2020, 6, 15, 12, 15, 3, DateTimeKind.Utc);
        private readonly DateTime datetime2 = new(2016, 12, 13, 14, 15, 16, DateTimeKind.Utc);

        [Fact]
        public void TestGetSetID()
        {
            // Arrange
            TurnEntity turn = new();
            Guid expected = Guid.NewGuid();

            // Act
            turn.ID = expected;
            Guid actual = turn.ID;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetWhen()
        {
            // Arrange
            TurnEntity turn = new();
            DateTime expected = datetime1;


            // Act
            turn.When = expected;
            DateTime actual = turn.When;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetPlayer()
        {
            // Arrange
            TurnEntity turn = new();
            PlayerEntity expected = player1;


            // Act
            turn.PlayerEntity = expected;
            PlayerEntity actual = turn.PlayerEntity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetPlayerID()
        {
            // Arrange
            TurnEntity turn = new();
            Guid expected = player1.ID;

            // Act
            turn.PlayerEntityID = expected;
            Guid actual = turn.PlayerEntityID;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetDice()
        {
            // Arrange
            TurnEntity turn = new();


            ICollection<DieEntity> expected = new List<DieEntity>
            {
                numDie,
                clrDie,
                imgDie
            };

            // Act
            turn.Dice = expected;
            ICollection<DieEntity> actual = turn.Dice;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetDieTurns()
        {
            // Arrange
            TurnEntity turn = new();
            List<DieTurn> expected = new() { dieTurn1, dieTurn2, dieTurn3 };


            // Act
            turn.DieTurns = expected;
            List<DieTurn> actual = turn.DieTurns;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetFaces()
        {
            // Arrange
            TurnEntity turn = new();


            ICollection<FaceEntity> expected = new List<FaceEntity>
            {
                numFace1,
                clrFace1,
                imgFace1
            };

            // Act
            turn.Faces = expected;
            ICollection<FaceEntity> actual = turn.Faces;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetFaceTurns()
        {
            // Arrange
            TurnEntity turn = new();
            List<FaceTurn> expected = new() { faceTurn1, faceTurn2 };


            // Act
            turn.FaceTurns = expected;
            List<FaceTurn> actual = turn.FaceTurns;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestEqualsWhenNotTurnEntityThenFalse()
        {
            // Arrange
            Point point;
            TurnEntity entity;

            // Act
            point = new(1, 2);
            entity = turn1;

            // Assert
            Assert.False(point.Equals(entity));
            Assert.False(entity.Equals(point));
        }

        [Fact]
        public void TestEqualsWhenNullThenFalse()
        {
            // Arrange
            TurnEntity entity;

            // Act
            entity = turn2;

            // Assert
            Assert.False(entity.Equals(null));
        }

        [Fact]
        public void TestGoesThruToSecondMethodIfObjIsTypeTurnEntity()
        {
            // Arrange
            object t1;
            TurnEntity t2;

            // Act
            t1 = turn1;
            t2 = turn2;

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t2.Equals(t1));
        }

        [Fact]
        public void TestEqualsFalseIfNotSame()
        {
            // Arrange
            TurnEntity t1;
            TurnEntity t2;

            // Act

            t1 = turn1;
            t2 = turn2;

            // Assert
            Assert.False(t1.Equals(t2));
            Assert.False(t2.Equals(t1));
        }

        [Fact]
        public void TestEqualsTrueIfSame()
        {
            // Arrange
            TurnEntity t1;
            TurnEntity t2;

            // Act

            t1 = turn2;
            t2 = turn3; // turns 2 and 3 should be same as far as Equals is concerned

            // Assert
            Assert.True(t1.Equals(t2));
            Assert.True(t2.Equals(t1));
        }

        [Fact]
        public void TestSameHashFalseIfNotSame()
        {
            // Arrange
            TurnEntity t1;
            TurnEntity t2;

            // Act

            t1 = turn1;
            t2 = turn2;

            // Assert
            Assert.False(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.False(t2.GetHashCode().Equals(t1.GetHashCode()));
        }

        [Fact]
        public void TestSameHashTrueIfSame()
        {
            // Arrange
            TurnEntity t1;
            TurnEntity t2;

            // Act

            t1 = turn2;
            t2 = turn3;

            // Assert
            Assert.True(t1.GetHashCode().Equals(t2.GetHashCode()));
            Assert.True(t2.GetHashCode().Equals(t1.GetHashCode()));
        }
    }
}
