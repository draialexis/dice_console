using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext, ILoader
    {
        // will be async!
        public virtual Task<MasterOfCeremonies> LoadApp() { throw new NotImplementedException(); }

        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<NumberDieEntity> NumberDice { get; set; }
        public DbSet<NumberFaceEntity> NumberFaces { get; set; }
        public DbSet<ImageDieEntity> ImageDice { get; set; }
        public DbSet<ImageFaceEntity> ImageFaces { get; set; }
        public DbSet<ColorDieEntity> ColorDice { get; set; }
        public DbSet<ColorFaceEntity> ColorFaces { get; set; }

        public DiceAppDbContext() { }

        public DiceAppDbContext(DbContextOptions<DiceAppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db").EnableSensitiveDataLogging();
        }
    }
}
