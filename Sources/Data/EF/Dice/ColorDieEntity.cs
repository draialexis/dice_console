using Data.EF.Dice.Faces;
using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice
{
    public class ColorDieEntity
    {
        public Guid Id { get; set; }
        public ICollection<ColorFaceEntity> Faces { get; set; }
    }
}
