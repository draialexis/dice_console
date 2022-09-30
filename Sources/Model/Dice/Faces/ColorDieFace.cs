using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class ColorDieFace : AbstractDieFace<Color>
    {
        
        /// <summary>
        /// </summary>
        /// <param name="hexValueString">Color type</param>
        public ColorDieFace(Color hexValueString):base(hexValueString)
        {}


        public override Color GetPracticalValue()
        {
            // https://stackoverflow.com/questions/1139957/convert-integer-to-hexadecimal-and-back-again
            return Value;
        }
    }
}
