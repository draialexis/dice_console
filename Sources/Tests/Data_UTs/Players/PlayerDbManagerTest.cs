using Data.EF;
using Data.EF.Players;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Tests.Data_UTs.Players
{
    public class PlayerDbManagerTest
    {


        private readonly SqliteConnection connection = new("DataSource=:memory:");
        private readonly DbContextOptions<DiceAppDbContext> options;

        public PlayerDbManagerTest()
        {
            connection.Open();

            options = new DbContextOptionsBuilder<DiceAppDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;
        }


        [Fact]
        public void TestConstructorWhenGivenContextThenConstructs()
        {
            // Arrange

            PlayerDbManager mgr;
            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                // Assert

                Assert.Equal(new Collection<PlayerEntity>(), mgr.GetAll());
            }
        }

        [Fact]
        public void TestConstructorWhenGivenNullThrowsException()
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            void action() => mgr = new PlayerDbManager(null);

            // Assert

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestAddWhenValidThenValid()
        {
            // Arrange

            string expectedName = "Jeff";
            int expectedCount = 2;
            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                mgr.Add(new PlayerEntity() { Name = expectedName });
                mgr.Add(new PlayerEntity() { Name = "whatever" });
                // mgr takes care of the SaveChange() calls internally
                // we might use Units of Work later, to optimize our calls to DB
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                Assert.Equal(expectedName, mgr.GetOneByName(expectedName).Name);
                Assert.Equal(expectedCount, mgr.GetAll().Count());
            }
        }


        [Fact]
        public void TestAddWhenPreExistentThenException()
        {
            // Arrange

            string name = "Flynt";
            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new PlayerEntity() { Name = name });
                void action() => mgr.Add(new PlayerEntity() { Name = name });

                // Assert

                Assert.Throws<ArgumentException>(action);
            }
        }


        [Fact]
        public void TestAddWhenNullThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                void action() => mgr.Add(null);

                // Assert

                Assert.Throws<ArgumentNullException>(action);
            }
        }


        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("")]
        public void TestAddWhenInvalidNameThenException(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                void action() => mgr.Add(new PlayerEntity() { Name = name });

                // Assert

                Assert.Throws<ArgumentException>(action);
            }
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("")]
        public void TestGetOneByNameWhenInvalidThenException(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { Name = "Ernesto" });
                mgr.Add(new() { Name = "Basil" });

                void action() => mgr.GetOneByName(name);

                // Assert

                Assert.Throws<ArgumentException>(action);
            }
        }

        [Theory]
        [InlineData("Caroline")]
        [InlineData("Caroline ")]
        [InlineData("  Caroline")]
        public void TestGetOneByNameWhenValidAndExistsThenGetsIt(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity actual;
            PlayerEntity expected = new() { Name = name.Trim() };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(expected);
                mgr.Add(new() { Name = "Philip" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                actual = mgr.GetOneByName(name);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void TestGetOneByNameWhenValidAndNotExistsThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                //mgr.Add(expected);
                mgr.Add(new() { Name = "Brett" });
                mgr.Add(new() { Name = "Noah" });

                void action() => mgr.GetOneByName("*r^a*éàru é^à");

                // Assert

                Assert.Throws<InvalidOperationException>(action);
            }
        }

        [Fact]
        public void TestIsPresentByNameWhenValidAndExistsThenTrue()
        {
            // Arrange

            PlayerDbManager mgr;
            string name = "Gerald";

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { Name = "Philip" });
                mgr.Add(new() { Name = name });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.True(mgr.IsPresentByName(name));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Barbara")]
        public void TestIsPresentByNameWhenInvalidOrNonExistentThenFalse(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { Name = "Herman" });
                mgr.Add(new() { Name = "Paulo" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.False(mgr.IsPresentByName(name));
            }
        }

        [Fact]
        public void TestRemoveWhenNullThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                void action() => mgr.Remove(null);

                // Assert

                Assert.Throws<ArgumentNullException>(action);
            }
        }

        [Fact]
        public void TestRemoveWhenPreExistentThenRemoves()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity toRemove = new() { Name = "Filibert" };

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { Name = "Xavier" });
                mgr.Add(toRemove);
            }

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Remove(toRemove);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(toRemove, mgr.GetAll());
            }
        }

        [Fact]
        public void TestRemoveWhenNonExistentThenStillNonExistent()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity toRemove = new() { Name = "Filibert" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { Name = "Bert" });
                mgr.Remove(toRemove);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(toRemove, mgr.GetAll());
            }
        }

        [Theory]
        [InlineData("filiBert")]
        [InlineData("Bertrand")]
        public void TestUpdateWhenValidThenUpdates(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            Guid idBefore = Guid.NewGuid();
            PlayerEntity before = new() { ID = idBefore, Name = "Filibert" };
            PlayerEntity after = new() { ID = idBefore, Name = name };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(before);
                mgr.Update(before, after);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(before, mgr.GetAll());
                Assert.Contains(after, mgr.GetAll());
            }
        }

        [Theory]
        [InlineData("Valerie")]
        [InlineData("Valerie ")]
        [InlineData(" Valerie")]
        public void TestUpdateWhenSameThenKeepsAndWorks(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            string nameBefore = "Valerie";
            Guid idBefore = Guid.NewGuid();
            PlayerEntity before = new() { ID = idBefore, Name = nameBefore };
            PlayerEntity after = new() { ID = idBefore, Name = name };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(before);
                mgr.Update(before, after);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.Contains(before, mgr.GetAll());
                Assert.Contains(after, mgr.GetAll());
            }
        }


        [Fact]
        public void TestUpdateWhenNewIDThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity before = new() { ID = Guid.NewGuid(), Name = "Nova" };
            PlayerEntity after = new() { ID = Guid.NewGuid(), Name = "Jacquie" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(before);
                void action() => mgr.Update(before, after);

                // Assert

                Assert.Throws<ArgumentException>(action);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void TestUpdateWhenInvalidThenException(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            Guid id = Guid.NewGuid();
            PlayerEntity before = new() { ID = id, Name = "Llanfair­pwll­gwyn­gyll­go­gery­chwyrn­drobwll­llan­tysilio­gogo­goch" };
            PlayerEntity after = new() { ID = id, Name = name };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(before);
                void action() => mgr.Update(before, after);

                // Assert

                Assert.Throws<ArgumentException>(action);
            }
        }

        [Fact]
        public void TestUpdateWhenNullThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity before = new() { ID = Guid.NewGuid(), Name = "Dwight" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(before);
                void action() => mgr.Update(before, null);

                // Assert

                Assert.Throws<ArgumentNullException>(action);
            }
        }

        [Fact]
        public void TestGetOneByIDWhenExistsThenGetsIt()
        {
            // Arrange

            PlayerDbManager mgr;

            Guid id = Guid.NewGuid();
            PlayerEntity actual;
            PlayerEntity expected = new() { ID = id, Name = "Hugh" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(expected);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                actual = mgr.GetOneByID(id);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void TestGetOneByIDWhenNotExistsThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            Guid id = Guid.NewGuid();
            PlayerEntity expected = new() { ID = id, Name = "Kyle" };

            Guid otherId = Guid.NewGuid();

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(expected);

                void action() => mgr.GetOneByID(otherId);

                // Assert

                Assert.Throws<InvalidOperationException>(action);
            }
        }

        [Fact]
        public void TestIsPresentbyIdWhenExistsThenTrue()
        {
            // Arrange

            PlayerDbManager mgr;
            Guid id = Guid.NewGuid();

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { ID = id, Name = "Bobby" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.True(mgr.IsPresentByID(id));
            }
        }

        [Fact]
        public void TestIsPresentbyIdWhenExistsThenFalse()
        {
            // Arrange

            PlayerDbManager mgr;
            Guid id = Guid.NewGuid();

            Guid otherId = Guid.NewGuid();

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                mgr.Add(new() { ID = id, Name = "Victor" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.False(mgr.IsPresentByID(otherId));
            }
        }
    }
}
