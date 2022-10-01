using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    internal class DiceAppDbContext : DbContext
    {
        public DbSet<PlayerEntity>? Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db");

    }
}
