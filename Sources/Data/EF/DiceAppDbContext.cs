using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext, ILoader
    {
        public virtual Task<MasterOfCeremonies> LoadApp() { throw new NotImplementedException(); }

        public DbSet<PlayerEntity> Players { get; set; }

        public DiceAppDbContext() { }

        public DiceAppDbContext(DbContextOptions<DiceAppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db").EnableSensitiveDataLogging();
        }
    }
}
