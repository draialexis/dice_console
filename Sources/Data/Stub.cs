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
            string g1 = "game1", g2 = "game2", g3 = "game3";

            Player player1 = new("Alice"), player2 = new("Bob"), player3 = new("Clyde");

            IManager<(string, IEnumerable<AbstractDie<AbstractDieFace>>)> globalDieManager = new DieManager();

            // create at least one fav group in there
            // ... 
            IEnumerable<AbstractDie<AbstractDieFace>> dice1;
            (_, dice1) = globalDieManager.GetAll().First();
            IEnumerable<AbstractDie<AbstractDieFace>> dice2;
            (_, dice2) = globalDieManager.GetAll().Last();

            Game game1 = new(name: g1, playerManager: new PlayerManager(), dice: dice1);
            Game game2 = new(name: g2, playerManager: new PlayerManager(), dice: dice2);
            Game game3 = new(name: g3, playerManager: new PlayerManager(), dice: dice1);

            List<Game> games = new() { game1, game2, game3 };

            PlayerManager globalPlayerManager = new();
            globalPlayerManager.Add(player1);
            globalPlayerManager.Add(player2);
            globalPlayerManager.Add(player3);

            GameRunner gameRunner = new(globalPlayerManager, globalDieManager, games);

            game1.AddPlayerToGame(player1);
            game1.AddPlayerToGame(player2);

            game2.AddPlayerToGame(player1);
            game2.AddPlayerToGame(player2);
            game2.AddPlayerToGame(player3);

            game3.AddPlayerToGame(player1);
            game3.AddPlayerToGame(player3);

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

        public static List<Player> LoadPlayers()
        {
            List<Player> list = new()
            {
                new Player("name 1"),
                new Player("name 2"),
                new Player("name 3"),
                new Player("name 4"),
                new Player("name 5"),
                new Player("name 6")
            };
            return list;
        }

        public static List<NumberDieFace> LoadNumFaces()
        {
            List<NumberDieFace> list = new()
            {
                new NumberDieFace(1),
                new NumberDieFace(2),
                new NumberDieFace(3),
                new NumberDieFace(4),
                new NumberDieFace(5),
                new NumberDieFace(6),
                new NumberDieFace(7)
            };

            return list;
        }

        public static List<ColorDieFace> LoadClrFaces()
        {
            List<ColorDieFace> list = new()
            {
                new ColorDieFace("ffffff"),
                new ColorDieFace("ffff66"),
                new ColorDieFace("ffff11"),
                new ColorDieFace("ffff22"),
                new ColorDieFace("ffff33"),
                new ColorDieFace("ffff44"),
                new ColorDieFace("ffff55")
            };

            return list;
        }

        public static List<ImageDieFace> LoadImgFaces()
        {
            string urlBase = "baseUrl/img/";
            List<ImageDieFace> list = new()
            {
                new ImageDieFace( urlBase + 1),
                new ImageDieFace( urlBase + 2),
                new ImageDieFace( urlBase + 3),
                new ImageDieFace( urlBase + 4),
                new ImageDieFace( urlBase + 5),
                new ImageDieFace( urlBase + 6),
                new ImageDieFace( urlBase + 7),
            };

            return list;
        }
    }
}