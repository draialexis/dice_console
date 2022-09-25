using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public abstract class AbstractDieFace
    {
        /// <summary>
        /// every die face has a value, and they can all be represented by an int,
        /// even if they're not litterally a decimal number
        /// <br/>
        /// USE GetPracticalValue for a Value specific to face type
        /// </summary>
        public int Value { get; protected set; }

        public abstract object GetPracticalValue();

        public override string ToString()
        {
            return GetPracticalValue().ToString();
        }
    }
}
