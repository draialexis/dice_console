using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class ImageDie : AbstractDie<ImageDieFace>
    {

        public ImageDie(string name, params ImageDieFace[] faces) : base(name, faces)
        {
        }
        public override ImageDieFace GetRandomFace()
        {
            Random rnd = new();
            int faceIndex = rnd.Next(1, ListFaces.Count() + 1);
            return ListFaces.ElementAt(faceIndex);
        }
    }
}
