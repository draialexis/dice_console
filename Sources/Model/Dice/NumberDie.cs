using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class NumberDie : AbstractDie<NumberDieFace>
    {

        public NumberDie(string name, params NumberDieFace[] faces) : base(name, faces)
        {
        }

    }
}
