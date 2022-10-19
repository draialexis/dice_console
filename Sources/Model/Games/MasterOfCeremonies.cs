using Model.Dice;
using Model.Players;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Games
{
    public class MasterOfCeremonies
    {
        public IManager<Player> GlobalPlayerManager { get; private set; }
        public IManager<DiceGroup> DiceGroupManager { get; private set; }
        public IManager<Game> GameManager { get; private set; }

        public MasterOfCeremonies(IManager<Player> globalPlayerManager, IManager<DiceGroup> globalDiceGroupManager, IManager<Game> gameManager)
        {
            GlobalPlayerManager = globalPlayerManager;
            DiceGroupManager = globalDiceGroupManager;
            GameManager = gameManager;
        }

        /// <summary>
        /// creates a new game
        /// </summary>
        /// <param name="name"></param>
        /// <param name="playerManager"></param>
        /// <param name="dice"></param>
        /// <returns></returns>
        public async Task<Game> StartNewGame(string name, IManager<Player> playerManager, IEnumerable<Die> dice)
        {
            Game game = new(name, playerManager, dice);
            return await GameManager.Add(game);
        }

        /// <summary>
        /// plays one turn of the game
        /// </summary>
        /// <param name="game">the game from which a turn will be played</param>
        public static async Task PlayGame(Game game)
        {
            Player current = await game.GetWhoPlaysNow();
            game.PerformTurn(current);
            await game.PrepareNextPlayer(current);
        }

    }
}
