using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;
using System.Security.Cryptography.X509Certificates;

namespace Data.EF
{
    public class DiceAppDbContextWithStub : DiceAppDbContext
    {
        public override GameRunner LoadApp()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerEntity>().HasData(
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Alice" },
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Bob" },
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Clyde" },
            new PlayerEntity { ID = Guid.NewGuid(), Name = "Dahlia" }
            );
        }
    }
}
