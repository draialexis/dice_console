using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dice;
using Model.Dice.Faces;
using Model.Players;

namespace Model.Games
{
    public class GameRunner
    {
        private readonly IManager<Player> globalPlayerManager;
        private readonly IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> globalDieManager;
        private readonly List<Game> games;

        public GameRunner(IManager<Player> globalPlayerManager, IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> globalDieManager, List<Game> games)
        {
            this.globalPlayerManager = globalPlayerManager;
            this.globalDieManager = globalDieManager;
            this.games = games ?? new();
        }

        public IEnumerable<Game> GetAllGames() => games.AsEnumerable();

        /// <summary>
        /// finds the game with that name and returns it
        /// <br/>
        /// that copy does not belong to this gamerunner's games, so it should not be modified 
        /// </summary>
        /// <param name="name">a games's name</param>
        /// <returns>game with said name, <em>or null</em> if no such game was found</returns>
        public Game GetOneGameByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Game result = games.FirstOrDefault(g => g.Name == name);
                return result; // may return null
            }
            throw new ArgumentException("param should not be null or blank", nameof(name));
        }

        /// <summary>
        /// saves a given game
        /// </summary>
        /// <param name="game">a game to save</param>
        /// <exception cref="NotSupportedException"></exception>
        public void SaveGame(Game game)
        {
            if(game != null)
            {
                games.Remove(games.FirstOrDefault(g => g.Name == game.Name));
                // will often be an update: if game with that name exists, it is removed, else, nothing happens above
                games.Add(game);
            }
            
        }

        /// <summary>
        /// loads a game by name to start playing again
        /// </summary>
        /// <param name="name">name of game to be loaded</param>
        /// <returns>loaded game</returns>
        /// <exception cref="NotSupportedException"></exception>
        public void LoadGame(string name)
        {
            Game game = GetOneGameByName(name);
            PlayGame(game);
        }

        /// <summary>
        /// creates a new game -- copies are OK
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public void StartNewGame(string name, PlayerManager playerManager, IEnumerable<AbstractDie<AbstractDieFace>> dice)
        {
            Game game = new(name, playerManager, dice);
            SaveGame(game);
            PlayGame(game);
        }

        public void DeleteGame(Game game)
        {
            games.Remove(game);
        }

        private void PlayGame(Game game)
        {
            while(true)
            {
                Player current = game.GetWhoPlaysNow();
                game.PerformTurn(current);
                game.PrepareNextPlayer(current);
                // TODO
                break;
            }
        }
    }
}
