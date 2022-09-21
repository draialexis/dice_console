using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
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
