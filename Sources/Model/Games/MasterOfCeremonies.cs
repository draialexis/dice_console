using Model.Dice;
using Model.Players;
using System.Collections.Generic;

namespace Model.Games
{
    public class MasterOfCeremonies
    {
        public IManager<Player> GlobalPlayerManager { get; private set; }
        public IManager<KeyValuePair<string, IEnumerable<Die>>> DieGroupManager { get; private set; }
        public IManager<Game> GameManager { get; private set; }

        public MasterOfCeremonies(IManager<Player> globalPlayerManager, IManager<KeyValuePair<string, IEnumerable<Die>>> globalDieManager, IManager<Game> gameManager)
        {
            GlobalPlayerManager = globalPlayerManager;
            DieGroupManager = globalDieManager;
            GameManager = gameManager;
        }

        /// <summary>
        /// creates a new game
        /// </summary>
        /// <param name="name"></param>
        /// <param name="playerManager"></param>
        /// <param name="dice"></param>
        /// <returns></returns>
        public Game StartNewGame(string name, IManager<Player> playerManager, IEnumerable<Die> dice)
        {
            Game game = new(name, playerManager, dice);
            return GameManager.Add(game);
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

    }
}
