using Model;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System.Collections.Generic;

namespace Data
{
    public class Stub : ILoader
    {

        // when the other classes are ready
        // the Stub should just make and return a GameRunner, and the GameRunner should have
        // a PlayerManager, a collection of Games, a FavGroupManager, etc. (see diagram)

        public GameRunner LoadApp()
        {
            IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> globalDieManager = new DieManager();

            List<AbstractDie<AbstractDieFace>> monopolyDice = new();
            List<AbstractDie<AbstractDieFace>> dndDice = new();

            string monopolyName = "Monopoly";
            string dndName = "DnD";

            NumberDieFace[] d6Faces = new NumberDieFace[] { new(1), new(2), new(3), new(4), new(5), new(6) };

            monopolyDice.Add(new NumberDie(d6Faces));
            monopolyDice.Add(new NumberDie(d6Faces));
            monopolyDice.Add(new ColorDie(new("#ff0000"), new("#00ff00"), new("#0000ff"), new("#ffff00"), new("#000000"), new("#ffffff")));

            NumberDieFace[] d20Faces = new NumberDieFace[] {
                new(1), new(2), new(3), new(4), new(5),
                new(6), new(7), new(8), new(9), new(10),
                new(11), new(12), new(13), new(14), new(15),
                new(16), new(17), new(18), new(19), new(20)
            };

            dndDice.Add(new NumberDie(d20Faces));

            globalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(monopolyName, monopolyDice.AsEnumerable()));
            globalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(dndName, dndDice.AsEnumerable()));

            IEnumerable<AbstractDie<AbstractDieFace>> dice1 = globalDieManager.GetOneByName(monopolyName).Value;
            IEnumerable<AbstractDie<AbstractDieFace>> dice2 = globalDieManager.GetOneByName(dndName).Value;

            string g1 = "game1", g2 = "game2", g3 = "game3";

            Game game1 = new(name: g1, playerManager: new PlayerManager(), dice: dice1);
            Game game2 = new(name: g2, playerManager: new PlayerManager(), dice: dice2);
            Game game3 = new(name: g3, playerManager: new PlayerManager(), dice: dice1);

            List<Game> games = new() { game1, game2, game3 };

            Player player1 = new("Alice"), player2 = new("Bob"), player3 = new("Clyde");

            PlayerManager globalPlayerManager = new();
            globalPlayerManager.Add(player1);
            globalPlayerManager.Add(player2);
            globalPlayerManager.Add(player3);

            GameRunner gameRunner = new(globalPlayerManager, globalDieManager, games);

            game1.PlayerManager.Add(player1);
            game1.PlayerManager.Add(player2);

            game2.PlayerManager.Add(player1);
            game2.PlayerManager.Add(player2);
            game2.PlayerManager.Add(player3);

            game3.PlayerManager.Add(player1);
            game3.PlayerManager.Add(player3);

            foreach (Game game in games)
            {
                for (int i = 0; i < 10; i++)
                {
                    Player currentPlayer = game.GetWhoPlaysNow();
                    game.PerformTurn(currentPlayer);
                    game.PrepareNextPlayer(currentPlayer);
                }
            }

            return gameRunner;
        }
    }
}