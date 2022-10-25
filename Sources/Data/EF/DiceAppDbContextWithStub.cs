using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Games;
using Data.EF.Joins;
using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Model.Games;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Data.EF
{
    public class DiceAppDbContextWithStub : DiceAppDbContext
    {
        // will be async
        public override Task<MasterOfCeremonies> LoadApp() { throw new NotImplementedException(); }

        public DiceAppDbContextWithStub() { }

        public DiceAppDbContextWithStub(DbContextOptions<DiceAppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Guid playerID_1 = Guid.NewGuid();
            Guid playerID_2 = new("6e856818-92f1-4d7d-b35c-f9c6687ef8e1");
            Guid playerID_3 = Guid.NewGuid();
            Guid playerID_4 = Guid.NewGuid();

            PlayerEntity player_1 = new() { ID = playerID_1, Name = "Alice" };
            PlayerEntity player_2 = new() { ID = playerID_2, Name = "Bob" };
            PlayerEntity player_3 = new() { ID = playerID_3, Name = "Clyde" };
            PlayerEntity player_4 = new() { ID = playerID_4, Name = "Dahlia" };

            Guid turnID_1 = Guid.NewGuid();
            Guid turnID_2 = Guid.NewGuid();

            DateTime datetime_1 = new(2017, 1, 6, 17, 30, 0, DateTimeKind.Utc);

            TurnEntity turn_1 = new() { ID = turnID_1, When = datetime_1, PlayerEntityID = playerID_1 };
            TurnEntity turn_2 = new() { ID = turnID_2, When = DateTime.UtcNow, PlayerEntityID = playerID_2 };

            Guid dieID_1 = Guid.NewGuid();
            Guid dieID_2 = Guid.NewGuid();
            Guid dieID_3 = Guid.NewGuid();

            NumberDieEntity die_1 = new() { ID = dieID_1 };
            ImageDieEntity die_2 = new() { ID = dieID_2 };
            ColorDieEntity die_3 = new() { ID = dieID_3 };

            Guid faceID_1 = Guid.NewGuid();
            Guid faceID_2 = Guid.NewGuid();
            Guid faceID_3 = Guid.NewGuid();
            Guid faceID_4 = Guid.NewGuid();
            Guid faceID_5 = Guid.NewGuid();
            Guid faceID_6 = Guid.NewGuid();

            NumberFaceEntity face_1 = new() { ID = faceID_1, Value = 1, NumberDieEntityID = dieID_1 };
            NumberFaceEntity face_2 = new() { ID = faceID_2, Value = 2, NumberDieEntityID = dieID_1 };

            ImageFaceEntity face_3 = new() { ID = faceID_3, Value = "https://1", ImageDieEntityID = dieID_2 };
            ImageFaceEntity face_4 = new() { ID = faceID_4, Value = "https://2", ImageDieEntityID = dieID_2 };

            ColorFaceEntity face_5 = new() { ID = faceID_5, ColorDieEntityID = dieID_3 };
            face_5.SetValue(Color.FromArgb(255, 255, 0, 0));
            ColorFaceEntity face_6 = new() { ID = faceID_6, ColorDieEntityID = dieID_3 };
            face_6.SetValue(Color.FromName("green"));

            modelBuilder.Entity<PlayerEntity>().HasData(player_1, player_2, player_3, player_4);

            modelBuilder.Entity<TurnEntity>().HasData(turn_1, turn_2);

            modelBuilder.Entity<NumberDieEntity>().HasData(die_1);
            modelBuilder.Entity<NumberFaceEntity>().HasData(face_1, face_2);

            modelBuilder.Entity<ImageDieEntity>().HasData(die_2);
            modelBuilder.Entity<ImageFaceEntity>().HasData(face_3, face_4);

            modelBuilder.Entity<ColorDieEntity>().HasData(die_3);
            modelBuilder.Entity<ColorFaceEntity>().HasData(face_5, face_6);

            //          die 1       die 2               die 3
            // turn 1 : num->2,     img->https://2,     clr->red
            // turn 2 : num->1,                         clr->green

            modelBuilder
                .Entity<TurnEntity>()
                .HasMany(turn => turn.Faces)
                .WithMany(face => face.Turns)
                .UsingEntity<FaceTurn>(join => join.HasData(
                    new { TurnEntityID = turnID_1, FaceEntityID = faceID_2 },
                    new { TurnEntityID = turnID_1, FaceEntityID = faceID_4 },
                    new { TurnEntityID = turnID_1, FaceEntityID = faceID_5 },
                    new { TurnEntityID = turnID_2, FaceEntityID = faceID_1 },
                    new { TurnEntityID = turnID_2, FaceEntityID = faceID_6 }));

            modelBuilder
                .Entity<TurnEntity>()
                .HasMany(turn => turn.Dice)
                .WithMany(die => die.Turns)
                .UsingEntity<DieTurn>(join => join.HasData(
                    new { TurnEntityID = turnID_1, DieEntityID = dieID_1 },
                    new { TurnEntityID = turnID_1, DieEntityID = dieID_2 },
                    new { TurnEntityID = turnID_1, DieEntityID = dieID_3 },
                    new { TurnEntityID = turnID_2, DieEntityID = dieID_1 },
                    new { TurnEntityID = turnID_2, DieEntityID = dieID_3 }));
        }
    }
}
