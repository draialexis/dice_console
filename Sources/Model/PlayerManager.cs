using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PlayerManager : IManager<Player>
    {
        public Player Add(ref Player toAdd)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Player> GetAll()
        {
            throw new NotImplementedException();
        }
        public Player GetOneById(int id)
        {
            throw new NotImplementedException();
        }
        public void Remove(ref Player toRemove)
        {
            throw new NotImplementedException();
        }
        public Player Update(ref Player before, ref Player after)
        {
            throw new NotImplementedException();
        }
    }
}
