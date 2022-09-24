using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dice;
using Model.Players;

namespace Model.Games
{
    public class GameRunner
    {
        private readonly PlayerManager globalPlayerManager;
        private readonly DieManager globalDieManager;
        private readonly List<Game> games;

        public GameRunner(PlayerManager globalPlayerManager, DieManager globalDieManager, List<Game> games)
        {
            this.globalPlayerManager = globalPlayerManager;
            this.globalDieManager = globalDieManager;
            this.games = games ?? new();
        }

        public IEnumerable<Game> GetAll() => games.AsEnumerable();

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
    }
}
