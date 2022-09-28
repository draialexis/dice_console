using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class ColorDieFace : AbstractDieFace
    {
        private static readonly int MAX_HEX = 16777215;
        /// <summary>
        /// accepts hex strings like "ffbb84" and "#af567d" ([RRGGBB])
        /// </summary>
        /// <param name="hexValueString">hex string</param>
        public ColorDieFace(string hexValueString)
        {
            // https://stackoverflow.com/questions/1139957/convert-integer-to-hexadecimal-and-back-again
            // we remove any initial '#' before parsing
            if (hexValueString.StartsWith('#'))
            {
                hexValueString = hexValueString[1..];
            }

            int result = int.Parse(hexValueString, System.Globalization.NumberStyles.HexNumber);

            if (result < 0) Value = 0;
            else if (result > MAX_HEX) Value = MAX_HEX;
            else Value = result;
        }

        /// <summary>
        /// accepts a decimal value that represents a color hex (0 is black, 65280 is green...)
        /// </summary>
        /// <param name="decimalValue"></param>
        public ColorDieFace(int decimalValue)
            : this(decimalValue.ToString())
        { }
        public override object GetPracticalValue()
        {
            // https://stackoverflow.com/questions/1139957/convert-integer-to-hexadecimal-and-back-again
            return Value.ToString("X6").Insert(0, "#");
        }
    }
}
