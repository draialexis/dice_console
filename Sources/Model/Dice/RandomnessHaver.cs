using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class RandomnessHaver
    {
        protected RandomnessHaver()
        {
        }
        protected static readonly Random rnd = new();
    }
}
