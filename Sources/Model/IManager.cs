using System.Collections.Generic;
namespace Model
{
    internal interface IManager
    {
        public T Add<T>(ref T toAdd);
        public void Remove<T>(ref T toRemove);
        public T Update<T>(ref T before, ref T after);
        public T GetOneById<T>(int id);
        public IEnumerable<T> GetAll<T>();
    }
}
