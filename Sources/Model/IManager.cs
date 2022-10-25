using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Model
{
    public interface IManager<T>
    {
        public Task<T> Add(T toAdd);

        public Task<T> GetOneByName(string name);

        public Task<T> GetOneByID(Guid ID);

        public Task<ReadOnlyCollection<T>> GetAll();

        public Task<T> Update(T before, T after);

        public void Remove(T toRemove);
    }
}
