using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Players
{
    public class PlayerManager : IManager<Player>
    {
        /// <summary>
        /// a hashset of the players that this manager is in charge of
        /// <br/>
        /// each player is unique (set), and the hash is based on the player's values (name)
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
                return toAdd;
            }
            throw new ArgumentNullException(nameof(toAdd), "param should not be null");
        }

        /// <summary>
        /// might never get implemented in the model, go through GetOneByName() in the meantime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Player GetOneById(int id)
        {
            throw new NotImplementedException("might never get implemented\ngo through GetOneByName() in the meantime");
        }

        /// <summary>
        /// finds the player with that name and returns A COPY OF IT
        /// <br/>
        /// that copy does not belong to this manager's players, so it should not be modified 
        /// </summary>
        /// <param name="name">a player's unique name</param>
        /// <returns>player with said name, <em>or null</em> if no such player was found</returns>
        public Player GetOneByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Player wanted = new(name);
                Player result = players.FirstOrDefault(p => p.Equals(wanted));
                return result is null ? null : new Player(result); // THIS IS A COPY (using a copy constructor)
            }
            throw new ArgumentException("param should not be null or blank", nameof(name));
        }

        /// </summary>
        /// get a READ ONLY enumerable of all players belonging to this manager
        /// so that the only way to modify the collection of players is to use this class's methods
        /// </summary>
        /// <returns>a readonly enumerable of all this manager's players</returns>
        public IEnumerable<Player> GetAll() => players.AsEnumerable();

        /// <summary>
        /// update a player from <paramref name="before"/> to <paramref name="after"/>
        /// </summary>
        /// <param name="before">player to be updated</param>
        /// <param name="after">player in the state that it needs to be in after the update</param>
        /// <returns>updated player</returns>
        public Player Update(Player before, Player after)
        {
            Player[] args = { before, after };

            foreach (Player player in args)
            {
                if (player is null)
                {
                    throw new ArgumentNullException(nameof(after), "param should not be null");
                    // could also be because of before, but one param had to be chosen as an example
                    // and putting "player" there was raising a major code smell
                }
            }
            Remove(before);
            return Add(after);
        }

        /// <summary>
        /// remove a player
        /// </summary>
        /// <param name="toRemove">player to be removed</param>
        public void Remove(Player toRemove)
        {
            if (toRemove is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            // the built-in HashSet.Remove() method will use our redefined Equals(), using Name only
            players.Remove(toRemove);
        }
    }
}
