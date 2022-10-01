using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Players
{
    [Index(nameof(Name), IsUnique = true)]
    public class PlayerEntity
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
    }
}
