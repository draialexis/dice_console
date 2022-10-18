using Data.EF.Dice.Faces;
using Data.EF.Dice;
using Data.EF.Players;
using Model.Dice;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Data.EF.Games;
using System.Drawing;
using Model.Games;
using Newtonsoft.Json.Linq;
using Model.Dice.Faces;
using System.Diagnostics;

namespace Tests.Data_UTs.Games
{
    public class TurnExtensionsTest
    {
        private readonly DateTime datetime1 = new(2013, 2, 1, 1, 19, 4, DateTimeKind.Utc);
        private readonly DateTime datetime2 = new(2014, 8, 1, 23, 12, 4, DateTimeKind.Utc);

        private readonly PlayerEntity playerEntity = new() { Name = "Paula" };

        private readonly DieEntity numDieEntity;
        private readonly FaceEntity numFace1Entity = new NumberFaceEntity() { Value = 7 };
        private readonly FaceEntity numFace2Entity = new NumberFaceEntity() { Value = 8 };

        private readonly DieEntity clrDieEntity;
        private readonly FaceEntity clrFace1Entity = new ColorFaceEntity() { A = 255, R = 255, G = 255, B = 255 };
        private readonly FaceEntity clrFace2Entity = new ColorFaceEntity() { A = 255, R = 0, G = 0, B = 128 };

        private readonly DieEntity imgDieEntity;
        private readonly FaceEntity imgFace1Entity = new ImageFaceEntity() { Value = "https://a" };
        private readonly FaceEntity imgFace2Entity = new ImageFaceEntity() { Value = "https://b" };

        public TurnExtensionsTest()
        {
            numDieEntity = new NumberDieEntity() { Faces = new List<NumberFaceEntity>() { numFace1Entity as NumberFaceEntity, numFace2Entity as NumberFaceEntity } };
            (numFace1Entity as NumberFaceEntity).NumberDieEntity = (NumberDieEntity)numDieEntity;
            (numFace2Entity as NumberFaceEntity).NumberDieEntity = (NumberDieEntity)numDieEntity;

            clrDieEntity = new ColorDieEntity() { Faces = new List<ColorFaceEntity>() { clrFace1Entity as ColorFaceEntity, clrFace2Entity as ColorFaceEntity } };
            (clrFace1Entity as ColorFaceEntity).ColorDieEntity = (ColorDieEntity)clrDieEntity;
            (clrFace2Entity as ColorFaceEntity).ColorDieEntity = (ColorDieEntity)clrDieEntity;

            imgDieEntity = new ImageDieEntity() { Faces = new List<ImageFaceEntity>() { imgFace1Entity as ImageFaceEntity, imgFace2Entity as ImageFaceEntity } };
            (imgFace1Entity as ImageFaceEntity).ImageDieEntity = (ImageDieEntity)imgDieEntity;
            (imgFace2Entity as ImageFaceEntity).ImageDieEntity = (ImageDieEntity)imgDieEntity;
        }

        [Fact]
        public void TestToModel()
        {
            // Arrange

            TurnEntity entity = new()
            {
                When = datetime1,
                PlayerEntity = playerEntity,
                Dice = new List<DieEntity>
                {
                    numDieEntity,
                    clrDieEntity,
                    imgDieEntity
                },
                Faces = new List<FaceEntity>
                {
                    numFace1Entity,
                    clrFace2Entity,
                    imgFace2Entity
                }
            };

            Turn expected = Turn.CreateWithSpecifiedTime(
                datetime1,
                playerEntity.ToModel(),
                new()
                {
                    {(numDieEntity as NumberDieEntity).ToModel(), (numFace1Entity as NumberFaceEntity).ToModel() },
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace2Entity as ColorFaceEntity).ToModel() },
                    {(imgDieEntity as ImageDieEntity).ToModel(), (imgFace2Entity as ImageFaceEntity).ToModel() }
                });

            // Act
            Turn actual = entity.ToModel();

            // Assert
            Assert.True(expected.Equals(actual));
        }

        [Fact]
        public void TestToModels()
        {
            // Arrange

            TurnEntity[] entities = new TurnEntity[]
            {
                new TurnEntity()
                {
                    When = datetime1,
                    PlayerEntity = new PlayerEntity() {Name = "Aardvark"},
                    Dice = new List<DieEntity>
                    {
                        numDieEntity,
                        clrDieEntity

                    },
                    Faces = new List<FaceEntity>
                    {
                        numFace2Entity,
                        clrFace2Entity
                    }
                },

                new TurnEntity()
                {
                    When = datetime2,
                    PlayerEntity = new PlayerEntity() {Name = "Chloe"},
                    Dice = new List<DieEntity>
                    {
                        clrDieEntity,
                        imgDieEntity
                    },
                    Faces = new List<FaceEntity>
                    {
                        clrFace1Entity,
                        imgFace1Entity
                    }
                }
            };

            IEnumerable<Turn> expected = new Turn[]
            {
                Turn.CreateWithSpecifiedTime(
                datetime1,
                new("Aardvark"),
                new()
                {
                    {(numDieEntity as NumberDieEntity).ToModel(), (numFace2Entity as NumberFaceEntity).ToModel() },
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace2Entity as ColorFaceEntity).ToModel() },
                }),

                Turn.CreateWithSpecifiedTime(
                datetime2,
                new("Chloe"),
                new()
                {
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace1Entity as ColorFaceEntity).ToModel() },
                    {(imgDieEntity as ImageDieEntity).ToModel(), (imgFace1Entity as ImageFaceEntity).ToModel() }
                })
            }.AsEnumerable();

            // Act
            IEnumerable<Turn> actual = entities.ToModels();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToEntity()
        {
            // Arrange

            Turn model = Turn.CreateWithSpecifiedTime(
                datetime1,
                playerEntity.ToModel(),
                new()
                {
                    {(numDieEntity as NumberDieEntity).ToModel(), (numFace2Entity as NumberFaceEntity).ToModel() },
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace2Entity as ColorFaceEntity).ToModel() },
                    {(imgDieEntity as ImageDieEntity).ToModel(), (imgFace1Entity as ImageFaceEntity).ToModel() }
                });

            TurnEntity expected = new()
            {
                When = datetime1,
                PlayerEntity = playerEntity,
                Dice = new List<DieEntity>
                {
                    numDieEntity,
                    clrDieEntity,
                    imgDieEntity
                },
                Faces = new List<FaceEntity>
                {
                    numFace2Entity,
                    clrFace2Entity,
                    imgFace1Entity
                }
            };

            // Act
            TurnEntity actual = model.ToEntity();

            // Assert
            Assert.True(expected.Equals(actual));
        }

        [Fact]
        public void TestToEntities()
        {
            // Arrange

            Turn[] models = new Turn[]
            {
                Turn.CreateWithSpecifiedTime(
                datetime2,
                new("Mimi"),
                new()
                {
                    {(numDieEntity as NumberDieEntity).ToModel(), (numFace2Entity as NumberFaceEntity).ToModel() },
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace2Entity as ColorFaceEntity).ToModel() },
                }),

                Turn.CreateWithSpecifiedTime(
                datetime1,
                new("blaaargh"),
                new()
                {
                    {(clrDieEntity as ColorDieEntity).ToModel(), (clrFace1Entity as ColorFaceEntity).ToModel() },
                    {(imgDieEntity as ImageDieEntity).ToModel(), (imgFace1Entity as ImageFaceEntity).ToModel() }
                })
            };

            IEnumerable<TurnEntity> expected = new TurnEntity[]
            {
                new TurnEntity()
                {
                    When = datetime2,
                    PlayerEntity = new PlayerEntity() {Name = "Mimi"},
                    Dice = new List<DieEntity>
                    {
                        numDieEntity,
                        clrDieEntity

                    },
                    Faces = new List<FaceEntity>
                    {
                        numFace2Entity,
                        clrFace2Entity
                    }
                },

                new TurnEntity()
                {
                    When = datetime1,
                    PlayerEntity = new PlayerEntity() {Name = "blaaargh"},
                    Dice = new List<DieEntity>
                    {
                        clrDieEntity,
                        imgDieEntity
                    },
                    Faces = new List<FaceEntity>
                    {
                        clrFace1Entity,
                        imgFace1Entity
                    }
                }
            }.AsEnumerable();

            // Act
            IEnumerable<TurnEntity> actual = models.ToEntities();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
