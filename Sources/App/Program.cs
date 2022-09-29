using Data;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ILoader loader = new Stub();
            GameRunner gameRunner;
            try
            {
                gameRunner = loader.LoadApp();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                gameRunner = new(new PlayerManager(), new DieManager(), null);
            }

            string menuChoice = "nothing";

            while (menuChoice != "q")
            {
                Console.WriteLine(
                    "l... load a game\n" +
                    "n... start new game\n" +
                    "d... delete a game\n" +
                    "c... create a group of dice\n" +
                    "q... QUIT\n" +
                    ">"
                    );

                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "q":
                        break;

                    case "l":
                        string loadName = ChooseGame(gameRunner);
                        if (gameRunner.GetOneByName(loadName) != null)
                        {
                            Play(gameRunner, loadName);
                        }
                        break;

                    case "n":

                        if (!gameRunner.GlobalDieManager.GetAll().Any())
                        {
                            Console.WriteLine("make at least one dice group first, then try again");
                            break;
                        }
                        IEnumerable<AbstractDie<AbstractDieFace>> newGameDice = PrepareDice(gameRunner);

                        string newGameName;
                        Console.WriteLine("give this new game a name\n>");
                        newGameName = Console.ReadLine();
                        PlayerManager playerManager = PreparePlayers(gameRunner);

                        gameRunner.StartNewGame(newGameName, playerManager, newGameDice);
                        Play(gameRunner, newGameName);

                        break;

                    case "d":
                        string deleteName = ChooseGame(gameRunner);
                        gameRunner.Remove(gameRunner.GetOneByName(deleteName));
                        break;

                    case "c":
                        string newGroupName;
                        Console.WriteLine("give this new dice group a name");
                        newGroupName = Console.ReadLine();

                        List<AbstractDie<AbstractDieFace>> newGroupDice = new();
                        string menuChoiceNewDice = "";
                        while (!(menuChoiceNewDice.Equals("ok") && newGroupDice.Any()))
                        {
                            AbstractDie<AbstractDieFace> die = null;
                            Console.WriteLine("create a die you want to add (at least one), or enter 'ok' if you're finished");
                            Console.WriteLine("what type of die ?\n" +
                                "n... number\n" +
                                "c... color\n" +
                                "i... image");
                            menuChoiceNewDice = Console.ReadLine();
                            switch (menuChoiceNewDice)
                            {
                                case "n":
                                    die = MakeNumberDie();
                                    break;

                                case "c":
                                    die = MakeColorDie();
                                    break;

                                case "i":
                                    die = MakeImageDie();
                                    break;
                            }
                            // almost no checks, this is temporary
                            if (die is not null)
                            {
                                newGroupDice.Add(die);
                            }
                        }
                        gameRunner.GlobalDieManager.Add(new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(newGroupName, newGroupDice));
                        break;

                    default:
                        Console.WriteLine("u wot m8?");
                        break;
                }
            }

        }

        private static void Play(GameRunner gameRunner, string name)
        {
            string menuChoicePlay = "";
            while (menuChoicePlay != "q")
            {
                Game game = gameRunner.GetOneByName(name);
                Console.WriteLine($"{game.GetWhoPlaysNow()}'s turn\n" +
                    "q... quit\n" +
                    "h... show history\n" +
                    "s... save\n" +
                    "any other... throw");
                menuChoicePlay = Console.ReadLine();
                switch (menuChoicePlay)
                {
                    case "q":
                        break;
                    case "h":
                        foreach (Turn turn in game.GetHistory())
                        {
                            Console.WriteLine(turn);
                        }
                        break;
                    case "s":
                        gameRunner.Add(game);
                        break;
                    default:
                        GameRunner.PlayGame(game);
                        Console.WriteLine(game.GetHistory().Last());
                        break;
                }
            }
        }

        private static string ChooseGame(GameRunner gameRunner)
        {
            string name;
            Console.WriteLine("which of these games?\n(choose by name)\n>");
            foreach (Game game in gameRunner.GetAll())
            {
                Console.WriteLine(game);
            }
            name = Console.ReadLine();
            return name;
        }

        private static NumberDie MakeNumberDie()
        {
            NumberDie die;
            List<NumberDieFace> faces = new();
            string menuChoiceNewFaces = "";

            while (menuChoiceNewFaces != "ok")
            {
                Console.WriteLine("create a face with a number, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();

                if (!menuChoiceNewFaces.Equals("ok") && int.TryParse(menuChoiceNewFaces, out int num))
                {
                    faces.Add(new(num));
                }
            }

            die = new NumberDie(faces.ToArray());
            return die;
        }

        private static ColorDie MakeColorDie()
        {
            ColorDie die;
            List<ColorDieFace> faces = new();
            string menuChoiceNewFaces = "";

            while (!menuChoiceNewFaces.Equals("ok"))
            {
                Console.WriteLine("create a face with an color hex code, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();
                if (menuChoiceNewFaces != "ok") faces.Add(new(menuChoiceNewFaces));
            }

            die = new ColorDie(faces.ToArray());
            return die;
        }

        private static ImageDie MakeImageDie()
        {
            ImageDie die;
            List<ImageDieFace> faces = new();
            string menuChoiceNewFaces = "";

            while (!menuChoiceNewFaces.Equals("ok"))
            {
                Console.WriteLine("create a face with an image url, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();

                if (menuChoiceNewFaces != "ok") faces.Add(new(menuChoiceNewFaces));
            }

            die = new ImageDie(faces.ToArray());
            return die;
        }

        private static IEnumerable<AbstractDie<AbstractDieFace>> PrepareDice(GameRunner gameRunner)
        {
            List<AbstractDie<AbstractDieFace>> result = new();
            Console.WriteLine("add dice to the game");
            Console.WriteLine("all known dice or groups of dice:");
            foreach ((string name, IEnumerable<AbstractDie<AbstractDieFace>> dice) in gameRunner.GlobalDieManager.GetAll())
            {
                Console.WriteLine($"{name} -- {dice}");
            }
            string menuChoiceDice = "";
            while (!(menuChoiceDice.Equals("ok") && result.Any()))
            {
                Console.WriteLine("write the name of a dice group you want to add (at least one), or 'ok' if you're finished");
                menuChoiceDice = Console.ReadLine();
                //  no checks, this is temporary
                if (!menuChoiceDice.Equals("ok"))
                {
                    IEnumerable<AbstractDie<AbstractDieFace>> chosenDice = gameRunner.GlobalDieManager.GetOneByName(menuChoiceDice).Value;
                    foreach (AbstractDie<AbstractDieFace> die in chosenDice)
                    {
                        result.Add(die);
                    }
                }
            }
            return result.AsEnumerable();
        }
        private static PlayerManager PreparePlayers(GameRunner gameRunner)
        {
            PlayerManager result = new();
            Console.WriteLine("add players to the game");
            Console.WriteLine("all known players:");
            foreach (Player player in gameRunner.GlobalPlayerManager.GetAll())
            {
                Console.WriteLine(player);
            }
            string menuChoicePlayers = "";
            while (!(menuChoicePlayers.Equals("ok") && result.GetAll().Any()))
            {
                Console.WriteLine("write the name of a player you want to add (at least one), or 'ok' if you're finished");
                menuChoicePlayers = Console.ReadLine();
                if (!menuChoicePlayers.Equals("ok"))
                {
                    Player player = new(menuChoicePlayers);
                    if (!gameRunner.GlobalPlayerManager.GetAll().Contains(player))
                    {
                        // if the player didn't exist, now it does... this is temporary
                        gameRunner.GlobalPlayerManager.Add(player);
                    }
                    // almost no checks, this is temporary
                    result.Add(player);
                }
            }

            return result;
        }
    }
}