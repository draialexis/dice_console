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
        private PlayerManager globalPlayerManager;
        private FavGroupManager favGroupManager;
        private IEnumerable<Game> games;

        public GameRunner(PlayerManager globalPlayerManager, FavGroupManager favGroupManager, IEnumerable<Game> games)
        {
            this.globalPlayerManager = globalPlayerManager;
            this.favGroupManager = favGroupManager;
            this.games = games;
        }
    }
}
