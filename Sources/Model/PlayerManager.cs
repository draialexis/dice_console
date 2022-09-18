using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class PlayerManager : IManager
    {
        public T Add<T>(ref T toAdd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public T GetOneById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(ref T toRemove)
        {
            throw new NotImplementedException();
        }

        public T Update<T>(ref T before, ref T after)
        {
            throw new NotImplementedException();
        }
    }
}
