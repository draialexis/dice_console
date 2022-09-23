using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class NumberDie : AbstractDie<NumberDieFace>
    {

        public NumberDie(string name, params NumberDieFace[] faces) : base(name, faces)
        {
        }
        public override NumberDieFace GetRandomFace()
        {
            Random rnd = new();
            int faceIndex = rnd.Next(1, ListFaces.Count() + 1);
            return ListFaces.ElementAt(faceIndex);

        }
    }
}
