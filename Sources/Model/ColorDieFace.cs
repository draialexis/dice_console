using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ColorDieFace : AbstractDieFace
    {
        /// <summary>
        /// a decimal representation of the hex (...representation of the color)
        /// </summary>
        protected override int Value { get; }

        /// <summary>
        /// accepts hex strings like "ffbb84" ([RRGGBB])
        /// </summary>
        /// <param name="hexValueString">hex string</param>
        public ColorDieFace(string hexValueString)
        {
            // https://stackoverflow.com/questions/1139957/convert-integer-to-hexadecimal-and-back-again

            // if style is ("f0b"), this constructor can develop it to "ff00bb" before doing the job

            Value = int.Parse(hexValueString, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// accepts a decimal value that represents a color hex (0 is black, 65280 is green...)
        /// </summary>
        /// <param name="decimalValue"></param>
        public ColorDieFace(int decimalValue)
        {
            Value = decimalValue;
        }
        public override object GetPracticalValue()
        {
            // https://stackoverflow.com/questions/1139957/convert-integer-to-hexadecimal-and-back-again
            // maybe prepend it with a "#"...
            return Value.ToString("X");
        }
    }
}
