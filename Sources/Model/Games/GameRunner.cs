using Model.Dice;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Games
{
    public class GameRunner : IManager<Game>
    {
        public IManager<Player> GlobalPlayerManager { get; private set; }
        public IManager<KeyValuePair<string, IEnumerable<Die>>> GlobalDieManager { get; private set; }
        private readonly List<Game> games;

        public GameRunner(IManager<Player> globalPlayerManager, IManager<KeyValuePair<string, IEnumerable<Die>>> globalDieManager, List<Game> games)
        {
            GlobalPlayerManager = globalPlayerManager;
            GlobalDieManager = globalDieManager;
            this.games = games ?? new();
        }

        public GameRunner(IManager<Player> globalPlayerManager, IManager<KeyValuePair<string, IEnumerable<Die>>> globalDieManager)
            : this(globalPlayerManager, globalDieManager, null) { }


        public IEnumerable<Game> GetAll() => games.AsEnumerable();

        /// <summary>
        /// finds the game with that name and returns it
        /// <br/>
        /// that copy does not belong to this gamerunner's games, so it should not be modified 
        /// </summary>
        /// <param name="name">a games's name</param>
        /// <returns>game with said name, <em>or null</em> if no such game was found</returns>
        public Game GetOneByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Game result = games.FirstOrDefault(g => g.Name == name);
                return result; // may return null
            }
            throw new ArgumentException("param should not be null or blank", nameof(name));
        }

        /// <summary>
        /// saves a given game -- does not allow copies yet: if a game with the same name exists, it is overwritten
        /// </summary>
        /// <param name="toAdd">a game to save</param>
        public Game Add(Game toAdd)
        {
            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");
            }

            games.Remove(games.FirstOrDefault(g => g.Name == toAdd.Name));
            // will often be an update: if game with that name exists, it is removed, else, nothing happens above
            games.Add(toAdd);
            return toAdd;
        }

        /// <summary>
        /// creates a new game
        /// </summary>
        public Game StartNewGame(string name, IManager<Player> playerManager, IEnumerable<Die> dice)
        {
            Game game = new(name, playerManager, dice);
            return Add(game);
        }

        public void Remove(Game toRemove)
        {
            if (toRemove is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            games.Remove(toRemove);
        }

        public Game Update(Game before, Game after)
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
            return Add(after);
        }

        /// <summary>
        /// plays one turn of the game
        /// </summary>
        /// <param name="game">the game from which a turn will be played</param>
        public static void PlayGame(Game game)
        {
            Player current = game.GetWhoPlaysNow();
            game.PerformTurn(current);
            game.PrepareNextPlayer(current);
        }

        public Game GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }
    }
}
