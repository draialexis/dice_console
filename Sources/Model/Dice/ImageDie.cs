using Model.Dice.Faces;
using System;

namespace Model.Dice
{
    public class ImageDie : HomogeneousDie<Uri>
    {
        public ImageDie(params ImageFace[] faces) : base(faces)
        {
        }
    }
}
