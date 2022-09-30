using Model.Dice.Faces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class ColorDie : AbstractDie<ColorDieFace,Color>
    {
        public ColorDie(params ColorDieFace[] faces) : base(faces)
        {
        }
    }
}
