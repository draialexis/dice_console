using System.Collections.Generic;
namespace Model
{
    public interface IManager<T>
    {
        public T Add(ref T toAdd);
        public T GetOneById(int id);
        public IEnumerable<T> GetAll();
        public T Update(ref T before, ref T after);
        public void Remove(ref T toRemove);
    }
}
