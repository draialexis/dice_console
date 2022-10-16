using Data.EF.Dice.Faces;
using Data.EF.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Joins
{
    public class FaceTurn
    {
        public Guid FaceEntityID { get; set; }
        public FaceEntity FaceEntity { get; set; }

        public Guid TurnEntityID { get; set; }
        public TurnEntity TurnEntity { get; set; }
    }
}
