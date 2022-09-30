using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dice.Faces;

namespace Model.Dice
{
    public class HomogeneousDice: AbstractDie<AbstractDieFace<object>,object>
    {
        public HomogeneousDice(params AbstractDieFace<object>[] faces) : base(faces)
        {

        }
    }
}
