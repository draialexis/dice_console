using Model.Dice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice.Faces
{
    public class NumberFaceEntity
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        [ForeignKey("NumDieFK")]
        public NumberDie NumberDie { get; set; }
    }
}
