using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Games
{
    public class GameManager : IManager<Game>
    {
        /// <summary>
        /// the games managed by this instance
        /// </summary>
        private readonly List<Game> games = new();


        /// <summary>
        /// gets an unmodifiable collection of the games
        /// </summary>
        /// <returns>unmodifiable collection of the games</returns>
        public Task<ReadOnlyCollection<Game>> GetAll() => Task.FromResult(new ReadOnlyCollection<Game>(games));

        /// <summary>
        /// finds the game with that name and returns it
        /// <br/>
        /// that copy does not belong to this manager's games, so it should not be modified 
        /// </summary>
        /// <param name="name">a games's name</param>
        /// <returns>game with said name, <em>or null</em> if no such game was found</returns>
        public Task<Game> GetOneByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Task.FromResult(games.FirstOrDefault(g => g.Name == name)); // may return null
            }
            throw new ArgumentException("param should not be null or blank", nameof(name));
        }

        /// <summary>
        /// not implemented in the model
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Game> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// saves a given game -- does not allow copies yet: if a game with the same name exists, it is overwritten
        /// </summary>
        /// <param name="toAdd">a game to save</param>
        public Task<Game> Add(Game toAdd)
        {
            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");
            }

            games.Remove(games.FirstOrDefault(g => g.Name == toAdd.Name));
            // will often be an update: if game with that name exists, it is removed, else, nothing happens above
            games.Add(toAdd);
            return Task.FromResult(toAdd);
        }

        /// <summary>
        /// removes a game. does nothing if the game doesn't exist
        /// </summary>
        /// <param name="toRemove">game to remove</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Remove(Game toRemove)
        {
            if (toRemove is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            games.Remove(toRemove);
        }

        /// <summary>
        /// updates a game
        /// </summary>
        /// <param name="before">original game</param>
        /// <param name="after">new game</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<Game> Update(Game before, Game after)
        {

            Game[] args = { before, after };

            foreach (Game game in args)
            {
                if (game is null)
                {
                    throw new ArgumentNullException(nameof(after), "param should not be null");
                    // could also be because of before, but one param had to be chosen as an example
                    // and putting "player" there was raising a major code smell
                }
            }
            Remove(before);
            return (Add(after));
        }
    }
}
