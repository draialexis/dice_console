using Data.EF.Dice.Faces;
using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice
{
    public class ImageDieEntity
    {
        public Guid Id { get; set; }
        public ICollection<ImageFaceEntity> Faces { get; set; }
    }
}
