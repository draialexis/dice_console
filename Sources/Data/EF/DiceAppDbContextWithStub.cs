using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    internal class DiceAppDbContextWithStub : DiceAppDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerEntity>().HasData(
                new PlayerEntity { ID = Guid.Parse("e3b42372-0186-484c-9b1c-01618fbfac44"), Name = "Alice" },
                new PlayerEntity { ID = Guid.Parse("73265e15-3c43-45f8-8f5d-d02feaaf7620"), Name = "Bob" },
                new PlayerEntity { ID = Guid.Parse("5198ba9d-44d6-4660-85f9-1843828c6f0d"), Name = "Clyde" },
                new PlayerEntity { ID = Guid.Parse("386cec27-fd9d-4475-8093-93c8b569bf2e"), Name = "Dahlia" }
                );
        }
    }
}
