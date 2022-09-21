using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ColorDieFace : AbstractDieFace
    {
        private string ColorHex;

        public ColorDieFace(string v)
        {
            this.ColorHex = v;
        }

        public string getColorHex() { return ColorHex; }
        public void setColorHex(string ColorHex) { this.ColorHex = ColorHex; }
    }
}
