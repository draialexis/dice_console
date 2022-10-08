using Model.Dice.Faces;
using System.Drawing;

namespace Model.Dice
{
    public class ColorDie : HomogeneousDie<Color>
    {
        public ColorDie(ColorFace first, params ColorFace[] faces) 
            : base(first, faces)
        {
        }
    }
}
