using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class NumberDieFace : AbstractDieFace<int>
    {
        public NumberDieFace(int value):base(value)
        {}

        public override int GetPracticalValue()
        {
            return Value;
        }
    }
}
