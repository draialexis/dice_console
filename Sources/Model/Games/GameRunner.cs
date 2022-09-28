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
        /// saves a given game -- does not allow copies yet: if a game with the same name exists, it is overwritten
        /// </summary>
        /// <param name="game">a game to save</param>
        /// <exception cref="NotSupportedException"></exception>
        public void SaveGame(Game game)
        {
            if (game != null)
            {
                games.Remove(games.FirstOrDefault(g => g.Name == game.Name));
                // will often be an update: if game with that name exists, it is removed, else, nothing happens above
                games.Add(game);
            }

        }

        /// <summary>
        /// creates a new game
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public void StartNewGame(string name, PlayerManager playerManager, IEnumerable<AbstractDie<AbstractDieFace>> dice)
        {
            Game game = new(name, playerManager, dice);
            SaveGame(game);
        }

        public void DeleteGame(Game game)
        {
            games.Remove(game);
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

        public void AddGlobalPlayer(Player player)
        {
            globalPlayerManager.Add(player);
        }

        public IEnumerable<Player> GetGlobalPlayers()
        {
            return globalPlayerManager.GetAll();
        }

        public Player GetOneGlobalPlayerByName(string name)
        {
            return globalPlayerManager.GetOneByName(name);
        }

        public void UpdateGlobalPlayer(Player oldPlayer, Player newPlayer)
        {
            globalPlayerManager.Update(oldPlayer, newPlayer);
        }

        public void DeleteGlobalPlayer(Player oldPlayer)
        {
            globalPlayerManager.Remove(oldPlayer);
        }

        public void AddGlobalDiceGroup(string name, IEnumerable<AbstractDie<AbstractDieFace>> dice)
        {
            globalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(name, dice));
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> GetGlobalDiceGroups()
        {
            return globalDieManager.GetAll();
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> GetOneGlobalDiceGroupByName(string name)
        {
            return globalDieManager.GetOneByName(name);
        }

        /// <summary>
        /// only updates names
        /// </summary>
        /// <param name="oldName">old name</param>
        /// <param name="newName">new name</param>
        public void UpdateGlobalDiceGroup(string oldName, string newName)
        {
            KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> oldDiceGroup = GetOneGlobalDiceGroupByName(oldName);
            KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> newDiceGroup = new(newName, oldDiceGroup.Value);
            globalDieManager.Update(oldDiceGroup, newDiceGroup);
        }

        /// <summary>
        /// will remove those dice groups from other games, potentially breaking them
        /// </summary>
        /// <param name="oldDiceGroup"></param>
        public void DeleteGlobalDiceGroup(string oldName)
        {
            globalDieManager.Remove(GetOneGlobalDiceGroupByName(oldName));
        }
    }
}
