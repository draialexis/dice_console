using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Data.EF
{
    public class DiceAppDbContextWithStub : DiceAppDbContext
    {
        public override Task<MasterOfCeremonies> LoadApp() { throw new NotImplementedException(); }

        public DiceAppDbContextWithStub() { }

        public DiceAppDbContextWithStub(DbContextOptions<DiceAppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerEntity>().HasData(
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Alice" }, // some tests depend on this name
            new PlayerEntity { ID = new("6e856818-92f1-4d7d-b35c-f9c6687ef8e1"), Name = "Bob" }, // some tests depend on this name
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Clyde" }, // some tests depend on this name
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Dahlia" } // some tests depend on this name
            );
        }
    }
}
