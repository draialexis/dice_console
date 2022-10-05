using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System.Drawing;

namespace Data
{
    public class Stub : ILoader
    {
        public GameRunner LoadApp()
        {
            GameRunner gr = new(new PlayerManager(), new DieManager());

            Player player1 = new("Alice(Old Stub)"), player2 = new("Bob(Old Stub)"), player3 = new("Clyde(Old Stub)");

            gr.GlobalPlayerManager.Add(player1);
            gr.GlobalPlayerManager.Add(player2);
            gr.GlobalPlayerManager.Add(player3);


            List<Die> monopolyDice = new();
            List<Die> dndDice = new();

            string monopolyName = "Monopoly", dndName = "DnD";

            NumberFace[] d6Faces = new NumberFace[] { new(1), new(2), new(3), new(4), new(5), new(6) };

            monopolyDice.Add(new NumberDie(new NumberFace(1), new NumberFace(1), new NumberFace(1), new NumberFace(1)));
            monopolyDice.Add(new NumberDie(d6Faces));
            monopolyDice.Add(new NumberDie(d6Faces));

            ColorFace[] colorFaces = new ColorFace[]
            {
                new(Color.FromName("blue")),
                new(Color.FromName("red")),
                new(Color.FromName("yellow")),
                new(Color.FromName("green")),
                new(Color.FromName("black")),
                new(Color.FromName("white"))
            };

            monopolyDice.Add(new ColorDie(colorFaces));

            string rootPath = "https://unsplash.com/photos/";

            ImageFace[] imageFaces = new ImageFace[]
            {
                new(new Uri(rootPath + "TLD6iCOlyb0")),
                new(new Uri(rootPath + "rTZW4f02zY8")),
                new(new Uri(rootPath + "Hyu76loQLdk")),
                new(new Uri(rootPath + "A_Ncbi-RH6s")),
            };

            monopolyDice.Add(new ImageDie(imageFaces));

            NumberFace[] d20Faces = new NumberFace[] {
                new(1), new(2), new(3), new(4), new(5),
                new(6), new(7), new(8), new(9), new(10),
                new(11), new(12), new(13), new(14), new(15),
                new(16), new(17), new(18), new(19), new(20)
            };

            dndDice.Add(new NumberDie(d20Faces));

            gr.GlobalDieManager.Add(new KeyValuePair<string, IEnumerable<Die>>(dndName, dndDice.AsEnumerable()));
            gr.GlobalDieManager.Add(new KeyValuePair<string, IEnumerable<Die>>(monopolyName, monopolyDice.AsEnumerable()));

            string game1 = "Forgotten Realms", game2 = "4e", game3 = "The Coopers";

            gr.Add(new(game1, new PlayerManager(), dndDice.AsEnumerable()));
            gr.Add(new(game2, new PlayerManager(), dndDice.AsEnumerable()));
            gr.Add(new(game3, new PlayerManager(), monopolyDice.AsEnumerable()));

            gr.GetOneByName(game1).PlayerManager.Add(player1);
            gr.GetOneByName(game1).PlayerManager.Add(player2);

            gr.GetOneByName(game2).PlayerManager.Add(player1);
            gr.GetOneByName(game2).PlayerManager.Add(player2);
            gr.GetOneByName(game2).PlayerManager.Add(player3);

            gr.GetOneByName(game3).PlayerManager.Add(player1);
            gr.GetOneByName(game3).PlayerManager.Add(player3);

            foreach (Game game in gr.GetAll())
            {
                for (int i = 0; i < 10; i++)
                {
                    Player currentPlayer = game.GetWhoPlaysNow();
                    game.PerformTurn(currentPlayer);
                    game.PrepareNextPlayer(currentPlayer);
                }
            }

            return gr;
        }
    }
}