using Model.Dice.Faces;
using System.Drawing;

namespace Model.Dice
{
    public class ColorDie : HomogeneousDie<Color>
    {
        public ColorDie(params ColorFace[] faces) : base(faces)
        {

        }
    }
}
