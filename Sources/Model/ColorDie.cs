using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ColorDie : AbstractDie<ColorDieFace>
    {
        public ColorDie(string name, params ColorDieFace[] faces) : base(name, faces)
        {
        }

        public override AbstractDieFace GetRandomFace()
        {
            Random rnd = new();
            int faceIndex = rnd.Next(1, ListFaces.Count() + 1);
            return ListFaces.ElementAt(faceIndex);
        }
    }
}
