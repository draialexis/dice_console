using System;
using System.Collections.Generic;
namespace Model
{
    public interface IManager<T>
    {
        public T Add(T toAdd);
        public T GetOneByName(string name);
        public T GetOneByID(Guid ID);
        public IEnumerable<T> GetAll();
        public T Update(T before, T after);
        public void Remove(T toRemove);
    }
}
