using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Players
{
    internal class PlayerDBManager : IManager<PlayerEntity>
    {
        PlayerDBContext context = new PlayerDBContext();
        
        public PlayerEntity Add(PlayerEntity toAdd)
        {
            // just to check...

            context.PlayersSet.Add(toAdd);
            throw new NotImplementedException();
        }

        public IEnumerable<PlayerEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public PlayerEntity GetOneByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(PlayerEntity toRemove)
        {
            throw new NotImplementedException();
        }

        public PlayerEntity Update(PlayerEntity before, PlayerEntity after)
        {
            throw new NotImplementedException();
        }
    }
}
