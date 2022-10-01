using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dice.Faces;

namespace Model.Dice
{
    public class HomogeneousDice<T>: AbstractDie<T>
    {
        public HomogeneousDice(params AbstractDieFace<T>[] faces) : base(faces)
        {

        }
    }
}
