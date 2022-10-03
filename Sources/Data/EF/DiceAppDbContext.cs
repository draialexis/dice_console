using Data.EF.Players;
using Microsoft.EntityFrameworkCore;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext
    {
        public DbSet<PlayerEntity>? Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db");

    }
}
