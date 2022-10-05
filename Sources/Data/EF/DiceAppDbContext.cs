using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext, ILoader
    {
        public virtual GameRunner LoadApp()
        {
            throw new NotImplementedException();
        }

        public DbSet<PlayerEntity>? Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db");

    }
}
