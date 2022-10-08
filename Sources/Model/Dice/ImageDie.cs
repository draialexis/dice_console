using Model.Dice.Faces;
using System;

namespace Model.Dice
{
    public class ImageDie : HomogeneousDie<Uri>
    {
        public ImageDie(ImageFace first, params ImageFace[] faces) 
            : base(first, faces)
        {
        }
    }
}
