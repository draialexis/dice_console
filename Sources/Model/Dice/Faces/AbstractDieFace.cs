using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public abstract class AbstractDieFace<T>
    {
        /// <summary>
        /// every die face has a value, and they can all be represented by an int,
        /// even if they're not litterally a decimal number
        /// <br/>
        /// USE GetPracticalValue for a Value specific to face type
        /// </summary>
        public T Value { get; protected set; }

        public abstract T GetPracticalValue();

        public AbstractDieFace(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return GetPracticalValue().ToString();
        }
    }
}
