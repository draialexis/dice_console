using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System.Drawing;

namespace Data
{
    public class Stub : ILoader
    {
        public async Task<MasterOfCeremonies> LoadApp()
        {
            MasterOfCeremonies mc = new(new PlayerManager(), new DiceGroupManager(), new GameManager());

            Player player1 = new("Alice(Old Stub)"), player2 = new("Bob(Old Stub)"), player3 = new("Clyde(Old Stub)");

            await mc.GlobalPlayerManager.Add(player1);
            await mc.GlobalPlayerManager.Add(player2);
            await mc.GlobalPlayerManager.Add(player3);


            List<Die> monopolyDice = new();
            List<Die> dndDice = new();

            string monopolyName = "Monopoly", dndName = "DnD";

            NumberFace[] d6Faces = new NumberFace[] { new(1), new(2), new(3), new(4), new(5), new(6) };

            monopolyDice.Add(new NumberDie(new NumberFace(1), new NumberFace(1), new NumberFace(1), new NumberFace(1)));
            monopolyDice.Add(new NumberDie(d6Faces[0], d6Faces[1..]));
            monopolyDice.Add(new NumberDie(d6Faces[0], d6Faces[1..]));

            ColorFace[] colorFaces = new ColorFace[]
            {
                new(Color.FromName("blue")),
                new(Color.FromName("red")),
                new(Color.FromName("yellow")),
                new(Color.FromName("green")),
                new(Color.FromName("black")),
                new(Color.FromName("white"))
            };

            monopolyDice.Add(new ColorDie(colorFaces[0], colorFaces[1..]));

            string rootPath = "https://unsplash.com/photos/";

            ImageFace[] imageFaces = new ImageFace[]
            {
                new(new Uri(rootPath + "TLD6iCOlyb0")),
                new(new Uri(rootPath + "rTZW4f02zY8")),
                new(new Uri(rootPath + "Hyu76loQLdk")),
                new(new Uri(rootPath + "A_Ncbi-RH6s")),
            };

            monopolyDice.Add(new ImageDie(imageFaces[0], imageFaces[1..]));

            NumberFace[] d20Faces = new NumberFace[] {
                new(1), new(2), new(3), new(4), new(5),
                new(6), new(7), new(8), new(9), new(10),
                new(11), new(12), new(13), new(14), new(15),
                new(16), new(17), new(18), new(19), new(20)
            };

            dndDice.Add(new NumberDie(d20Faces[0], d20Faces[1..]));

            await mc.DiceGroupManager.Add(new DiceGroup(dndName, dndDice));
            await mc.DiceGroupManager.Add(new DiceGroup(monopolyName, monopolyDice));

            string game1 = "Forgotten Realms", game2 = "4e", game3 = "The Coopers";

            await mc.GameManager.Add(new(game1, new PlayerManager(), dndDice.AsEnumerable()));
            await mc.GameManager.Add(new(game2, new PlayerManager(), dndDice.AsEnumerable()));
            await mc.GameManager.Add(new(game3, new PlayerManager(), monopolyDice.AsEnumerable()));

            await (await mc.GameManager.GetOneByName(game1)).PlayerManager.Add(player1);
            await (await mc.GameManager.GetOneByName(game1)).PlayerManager.Add(player2);

            await (await mc.GameManager.GetOneByName(game2)).PlayerManager.Add(player1);
            await (await mc.GameManager.GetOneByName(game2)).PlayerManager.Add(player2);
            await (await mc.GameManager.GetOneByName(game2)).PlayerManager.Add(player3);

            await (await mc.GameManager.GetOneByName(game3)).PlayerManager.Add(player1);
            await (await mc.GameManager.GetOneByName(game3)).PlayerManager.Add(player3);

            foreach (Game game in mc.GameManager.GetAll()?.Result)
            {
                for (int i = 0; i < 10; i++)
                {
                    Player currentPlayer = await game.GetWhoPlaysNow();
                    game.PerformTurn(currentPlayer);
                    await game.PrepareNextPlayer(currentPlayer);
                }
            }

            return mc;
        }
    }
}