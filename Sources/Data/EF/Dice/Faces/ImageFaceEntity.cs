using Model.Dice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice.Faces
{
    public class ImageFaceEntity
    {
        public Guid ID { get; set; }
        public string Value { get; set; }

        [ForeignKey("ImgDieFK")]
        public ImageDieEntity ImageDieEntity { get; set; }
    }
}
