using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal interface IManager
    {
        public T Add<T>(ref T toAdd);
        public T Remove<T>(ref T toRemove);
        public T Update<T>(ref T before, ref T after);
    }
}
