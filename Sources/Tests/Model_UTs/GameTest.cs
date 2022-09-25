using Model.Dice.Faces;
using Model.Dice;
using Model.Games;
using Model.Players;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Model_UTs
{
    public class GameTest
    {

        private static readonly string GAME_NAME = "my game";

        private static readonly Player PLAYER_1 = new("Alice"), PLAYER_2 = new("Bob"), PLAYER_3 = new("Clyde");

        private readonly static NumberDieFace[] FACES_1 = new NumberDieFace[]
        {
            new(1),
            new(2),
            new(3),
            new(4)
        };

        private readonly static ImageDieFace[] FACES_2 = new ImageDieFace[]
        {
            new(10),
            new(20),
            new(30),
            new(40)
        };

        private readonly static ColorDieFace[] FACES_3 = new ColorDieFace[]
        {
            new(1000),
            new(2000),
            new(3000),
            new(4000)
        };

        private static readonly AbstractDie<AbstractDieFace> NUM = new NumberDie(FACES_1), IMG = new ImageDie(FACES_2), CLR = new ColorDie(FACES_3);

        private static readonly IEnumerable<AbstractDie<AbstractDieFace>> dice =
            new List<AbstractDie<AbstractDieFace>>() { NUM, IMG, CLR }
            .AsEnumerable();

        public GameTest()
        {

            Game game = new(name: GAME_NAME, playerManager: new PlayerManager(), dice: dice);

            game.AddPlayerToGame(PLAYER_1);
            game.AddPlayerToGame(PLAYER_2);
            game.AddPlayerToGame(PLAYER_3);
        }
    }
}
