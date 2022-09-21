using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class NumberDieFace : AbstractDieFace
    {
        public int Num { get; set; }

        public NumberDieFace(int v)
        {
            this.Num = v;
        }

        

        
    }
}
