using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class AbstractDieFace
    {
        /// <summary>
        /// every die face has a value, and they can all be represented by an int,
        /// even if they're not litterally a decimal number
        /// <br/>
        /// USE GetPracticalValue for a Value specific to face type
        /// </summary>
        protected abstract int Value { get; }

        public abstract object GetPracticalValue();
    }
}
