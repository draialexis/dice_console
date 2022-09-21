using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Players
{
    public class PlayerManager : IManager<Player>
    {
        /// <summary>
        /// a collection of the players that this manager is in charge of
        /// </summary>
        private readonly List<Player> players;

        /// <summary>
        /// references the position in list of the current player, for a given game.
        /// <br/>
        /// ASSUMING that each Game made its own instance of PlayerManager
        /// </summary>
        public int NextIndex { get; private set; } = 0;

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
            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");
            }
            if (players.Contains(toAdd))
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            players.Add(toAdd);
            return toAdd;
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
        /// finds and returns the player whose turn it is
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        // TODO finish or start again
        public Player WhoPlaysNow(bool isFirstTurn)
        {
            if (players.Count == 0)
            {
                throw new Exception("you are exploring an empty collection\nthis should not have happened");
            }

            Player result;
            if (isFirstTurn)
            {
                result = players[0];
            }
            else
            {
                result = players[NextIndex];
            }
            return result;
        }

        /// <summary>
        /// this feels very dirty
        /// </summary>
        /// <param name="current"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void PrepareNextPlayer(Player current)
        {
            if (players.Count == 0)
            {
                throw new Exception("you are exploring an empty collection\nthis should not have happened");
            }
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current), "param should not be null");
            }
            if (!players.Contains(current))
            {
                throw new ArgumentException("param could not be found in this collection\n did you forget to add it?", nameof(current));
            }

            //if (currentIndex >= players.Count() - 1) 
            if (players.Last() == current)
            {
                // if we've reached the last index, we need to loop back around
                NextIndex = 0;
            }
            else
            {
                // else we can just move up one from current
                NextIndex++;
            }
        }

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
            // the built-in Remove() method will use our redefined Equals(), using Name only
            players.Remove(toRemove);
        }
    }
}
