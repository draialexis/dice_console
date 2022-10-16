using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Games;
using Data.EF.Joins;
using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext, ILoader
    {
        // will be async!
        public virtual Task<MasterOfCeremonies> LoadApp() { throw new NotImplementedException(); }

        public DbSet<PlayerEntity> PlayerEntity { get; set; }
        public DbSet<TurnEntity> TurnEntity { get; set; }
        public DbSet<NumberDieEntity> NumberDieEntity { get; set; }
        public DbSet<NumberFaceEntity> NumberFaceEntity { get; set; }
        public DbSet<ImageDieEntity> ImageDieEntity { get; set; }
        public DbSet<ImageFaceEntity> ImageFaceEntity { get; set; }
        public DbSet<ColorDieEntity> ColorDieEntity { get; set; }
        public DbSet<ColorFaceEntity> ColorFaceEntity { get; set; }

        public DiceAppDbContext() { }

        public DiceAppDbContext(DbContextOptions<DiceAppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db").EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // thanks to https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#join-entity-type-configuration

            // many to many TurnEntity <-> FaceEntity
            modelBuilder.Entity<FaceEntity>()
                .HasMany(face => face.Turns)
                .WithMany(turn => turn.Faces)
                .UsingEntity<FaceTurn>(

                    join => join
                    // FaceTurn --> TurnEntity
                        .HasOne(faceturn => faceturn.TurnEntity)
                        .WithMany(turn => turn.FaceTurns)
                        .HasForeignKey(faceturn => faceturn.TurnEntityID),
                    join => join
                    // FaceTurn --> FaceEntity
                        .HasOne(faceturn => faceturn.FaceEntity)
                        .WithMany(face => face.FaceTurns)
                        .HasForeignKey(faceturn => faceturn.FaceEntityID),
                    // FaceTurn.PK = (ID1, ID2)
                    join => join.HasKey(faceturn => new { faceturn.FaceEntityID, faceturn.TurnEntityID })

                    );

            // many to many TurnEntity <-> DieEntity
            modelBuilder.Entity<DieEntity>()
                .HasMany(die => die.Turns)
                .WithMany(turn => turn.Dice)
                .UsingEntity<DieTurn>(

                    join => join
                    // DieTurn --> TurnEntity
                        .HasOne(dieturn => dieturn.TurnEntity)
                        .WithMany(turn => turn.DieTurns)
                        .HasForeignKey(dieturn => dieturn.TurnEntityID),
                    join => join
                    // DieTurn --> DieEntity
                        .HasOne(dieturn => dieturn.DieEntity)
                        .WithMany(die => die.DieTurns)
                        .HasForeignKey(dieturn => dieturn.DieEntityID),
                    // DieTurn.PK = (ID1, ID2)
                    join => join.HasKey(dieturn => new { dieturn.DieEntityID, dieturn.TurnEntityID })

                    );
        }
    }
}
