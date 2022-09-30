using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public class DiceAppDbContext : DbContext
    {
        public DbSet<PlayerEntity> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db");

        /* test with this  
         
        > dotnet ef migrations add person_test

        > dotnet ef database update

        ...

        using (DiceAppDbContext db = new())
            {
                db.Players.AddRange(PlayerExtensions.ToEntities(new Player[] {
                new("Alice"),
                new("Bob"),
                new("Clyde"),
                new("Fucking Kevin GOSH")
            }));

                Console.WriteLine("Added, not saved");
                if (db.Players is not null)
                {
                    foreach (PlayerEntity p in db.Players)
                    {
                        Console.WriteLine(p.ID + " - " + p.Name);
                    }
                }

                db.SaveChanges();

                Console.WriteLine("Saved");
                foreach (PlayerEntity p in db.Players)
                {
                    Console.WriteLine(p.ID + " - " + p.Name);
                }
            }
         */


    }
}
