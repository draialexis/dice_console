using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class NumberDie : HomogeneousDice<int>
    {
        public NumberDie(params NumberDieFace[] faces) : base(faces)
        {
        }
    }
}
