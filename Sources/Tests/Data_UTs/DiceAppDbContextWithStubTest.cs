using Data.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Data_UTs
{
    public class DiceAppDbContextWithStubTest
    {

        private readonly SqliteConnection connection = new("DataSource=:memory:");
        private readonly DbContextOptions<DiceAppDbContext> options;

        public DiceAppDbContextWithStubTest()
        {
            connection.Open();

            options = new DbContextOptionsBuilder<DiceAppDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;
        }

        [Theory]
        [InlineData("Alice")]
        [InlineData("Bob")]
        [InlineData("Clyde")]
        [InlineData("Dahlia")]
        public void TestDbStubContainsAll(string name)
        {
            // Arrange

            using (DiceAppDbContextWithStub db = new(options))
            {
                db.Database.EnsureCreated();

                // Assert

                Assert.True(db.Players.Where(p => p.Name.Equals(name)).Any());
            }
        }

    }
}
