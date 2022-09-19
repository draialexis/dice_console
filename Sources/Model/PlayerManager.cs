using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PlayerManager : IManager<Player>
    {
        /// <summary>
        /// a hashset of the players that this manager is in charge of
        /// <br/>
        /// the hash is based on the player's unique property (name)
        /// </summary>
        private readonly HashSet<Player> players;

        public PlayerManager()
        {
            players = new();
        }
        /// <summary>
        /// add a new player
        /// </summary>
        /// <param name="toAdd">player to be added</param>
        /// <returns>added player, or null if <paramref name="toAdd"/> was null</returns>
        public Player Add(Player toAdd)
        {
            if (toAdd != null)
            {
                players.Add(toAdd);
            }
            return toAdd;
        }

        /// <summary>
        /// may never get implemented in the model, go through GetOneByName() in the meantime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Player GetOneById(int id)
        {
            throw new NotImplementedException("may never get implemented\ngo through GetOneByName() in the meantime");
        }

        /// <summary>
        /// finds the player with that name and returns A COPY OF IT
        /// <br/>
        /// that copy does not belong to this manager's players, so it should not be modified 
        /// </summary>
        /// <param name="name">a player's unique name</param>
        /// <returns>player with said name</returns>
        public Player GetOneByName(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                Player result = players.FirstOrDefault(p => p.Name.ToUpper().Equals(name.Trim().ToUpper()));
                if (result == null) return result; // will return null by default if no such player exists
                return new(result); // THIS IS A COPY (using a copy constructor)
            }
            return null; // we also want ot return null if no name was provided
        }

        /// </summary>
        /// get a READ ONLY enumerable of all players belonging to this manager
        /// </summary>
        /// <returns>a readonly list of all this manager's players</returns>
        public IEnumerable<Player> GetAll() => players.AsEnumerable();

        /// <summary>
        /// update a player from <paramref name="before"/> to <paramref name="after"/>
        /// </summary>
        /// <param name="before">player to be updated</param>
        /// <param name="after">player in the state that it needs to be in after the update</param>
        /// <returns>updated player</returns>
        public Player Update(Player before, Player after)
        {
            Remove(before);
            return Add(after);
        }

        /// <summary>
        /// remove a player
        /// </summary>
        /// <param name="toRemove">player to be removed</param>
        public void Remove(Player toRemove)
        {
            players.Remove(toRemove);
        }
    }
}
