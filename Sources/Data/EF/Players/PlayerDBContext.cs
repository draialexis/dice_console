using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Players
{
    internal class PlayerDBContext : DbContext
    {
        public DbSet<PlayerEntity> PlayersSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EFDice.DiceApp.db");
        }

    }
}
