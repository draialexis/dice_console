using Data.EF;
using Data.EF.Players;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
        public async Task TestConstructorWhenGivenContextThenConstructs()
        {
            // Arrange

            PlayerDbManager mgr;
            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                // Assert

                Assert.Equal(new Collection<PlayerEntity>(), await mgr.GetAll());
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
        public async Task TestAddWhenValidThenValid()
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
                await mgr.Add(new PlayerEntity() { Name = expectedName });
                await mgr.Add(new PlayerEntity() { Name = "whatever" });
                // mgr takes care of the SaveChange() calls internally
                // we might use Units of Work later, to optimize our calls to DB
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                Assert.Equal(expectedName, (await mgr.GetOneByName(expectedName)).Name);
                Assert.Equal(expectedCount, (await mgr.GetAll()).Count());
            }
        }


        [Fact]
        public async Task TestAddWhenPreExistentThenException()
        {
            // Arrange

            string name = "Flynt";
            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new PlayerEntity() { Name = name });
                async Task actionAsync() => await mgr.Add(new PlayerEntity() { Name = name });

                // Assert

                await Assert.ThrowsAsync<ArgumentException>(actionAsync);
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

                async Task actionAsync() => await mgr.Add(null);

                // Assert

                Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
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

                async Task actionAsync() => await mgr.Add(new PlayerEntity() { Name = name });

                // Assert

                Assert.ThrowsAsync<ArgumentException>(actionAsync);
            }
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("")]
        public async Task TestGetOneByNameWhenInvalidThenException(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new() { Name = "Ernesto" });
                await mgr.Add(new() { Name = "Basil" });

                async Task actionAsync() => await mgr.GetOneByName(name);

                // Assert

                await Assert.ThrowsAsync<ArgumentException>(actionAsync);
            }
        }

        [Theory]
        [InlineData("Caroline")]
        [InlineData("Caroline ")]
        [InlineData("  Caroline")]
        public async Task TestGetOneByNameWhenValidAndExistsThenGetsIt(string name)
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

                await mgr.Add(expected);
                await mgr.Add(new() { Name = "Philip" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                actual = await mgr.GetOneByName(name);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public async Task TestGetOneByNameWhenValidAndNotExistsThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                //mgr.Add(expected);
                await mgr.Add(new() { Name = "Brett" });
                await mgr.Add(new() { Name = "Noah" });

                async Task actionAsync() => await mgr.GetOneByName("*r^a*éàru é^à");

                // Assert

                await Assert.ThrowsAsync<InvalidOperationException>(actionAsync);
            }
        }

        [Fact]
        public async Task TestIsPresentByNameWhenValidAndExistsThenTrue()
        {
            // Arrange

            PlayerDbManager mgr;
            string name = "Gerald";

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new() { Name = "Philip" });
                await mgr.Add(new() { Name = name });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.True(await mgr.IsPresentByName(name));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("nowaythatthisnameisalreadyinourdatabase")]
        public async Task TestIsPresentByNameWhenInvalidOrNonExistentThenFalse(string name)
        {
            // Arrange

            PlayerDbManager mgr;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new() { Name = "Herman" });
                await mgr.Add(new() { Name = "Paulo" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.False(await mgr.IsPresentByName(name));
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

            PlayerEntity toRemove = new() { ID = Guid.NewGuid(), Name = "Please!" };


            // Act

            using (DiceAppDbContextWithStub db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                mgr.Add(toRemove); // calls SaveChangesAsync()
                mgr.Remove(toRemove);
            }

            // Assert

            using (DiceAppDbContextWithStub db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(toRemove, db.Players);
            }
        }

        [Fact]
        public async Task TestRemoveWhenNonExistentThenStillNonExistent()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity toRemove = new() { ID = Guid.NewGuid(), Name = "Filibert" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new() { ID = Guid.NewGuid(), Name = "Bert" });
                mgr.Remove(toRemove);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(toRemove, await mgr.GetAll());
            }
        }

        [Theory]
        [InlineData("filiBert")]
        [InlineData("Bertrand")]
        public async Task TestUpdateWhenValidThenUpdates(string name)
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

                await mgr.Add(before);
                await mgr.Update(before, after);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(before, await mgr.GetAll());
                Assert.Contains(after, await mgr.GetAll());
            }
        }

        [Theory]
        [InlineData("Valerie")]
        [InlineData("Valerie ")]
        [InlineData(" Valerie")]
        public async Task TestUpdateWhenSameThenKeepsAndWorks(string name)
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

                await mgr.Add(before);
                await mgr.Update(before, after);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.Contains(before, await mgr.GetAll());
                Assert.Contains(after, await mgr.GetAll());
            }
        }


        [Fact]
        public async Task TestUpdateWhenNewIDThenException()
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

                await mgr.Add(before);
                async Task actionAsync() => await mgr.Update(before, after);

                // Assert

                await Assert.ThrowsAsync<ArgumentException>(actionAsync);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task TestUpdateWhenInvalidThenException(string name)
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

                await mgr.Add(before);
                async Task actionAsync() => await mgr.Update(before, after);

                // Assert

                await Assert.ThrowsAsync<ArgumentException>(actionAsync);
            }
        }

        [Fact]
        public async Task TestUpdateWhenNullThenException()
        {
            // Arrange

            PlayerDbManager mgr;

            PlayerEntity before = new() { ID = Guid.NewGuid(), Name = "Dwight" };

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(before);
                async Task actionAsync() => await mgr.Update(before, null);

                // Assert

                await Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
            }
        }

        [Fact]
        public async Task TestGetOneByIDWhenExistsThenGetsIt()
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

                await mgr.Add(expected);
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);
                actual = await mgr.GetOneByID(id);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public async Task TestGetOneByIDWhenNotExistsThenExceptionAsync()
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

                await mgr.Add(expected);

                async Task actionAsync() => await mgr.GetOneByID(otherId);

                // Assert

                await Assert.ThrowsAsync<InvalidOperationException>(actionAsync);
            }
        }

        [Fact]
        public async Task TestIsPresentbyIdWhenExistsThenTrue()
        {
            // Arrange

            PlayerDbManager mgr;
            Guid id = Guid.NewGuid();

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                await mgr.Add(new() { ID = id, Name = "Bobby" });
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.True(await mgr.IsPresentByID(id));
            }
        }

        [Fact]
        public async Task TestIsPresentbyIdWhenNotExistsThenFalse()
        {
            // Arrange

            PlayerDbManager mgr;
            Guid id = Guid.NewGuid();

            Guid otherId = Guid.NewGuid();

            PlayerEntity presentEntity;
            PlayerEntity absentEntity;

            // Act

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                presentEntity = new() { ID = id, Name = "Victor" };
                await mgr.Add(presentEntity);

                absentEntity = new() { ID = otherId, Name = "Victor" };
            }

            // Assert

            using (DiceAppDbContext db = new(options))
            {
                db.Database.EnsureCreated();
                mgr = new(db);

                Assert.DoesNotContain(absentEntity, db.Players);
            }
        }
    }
}
