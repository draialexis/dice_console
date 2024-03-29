﻿using Data;
using Data.EF;
using Data.EF.Players;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal static class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            // MODEL stuff
            ILoader loader = new Stub();
            MasterOfCeremonies masterOfCeremonies;
            try
            {
                masterOfCeremonies = await loader.LoadApp();
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                masterOfCeremonies = new(new PlayerManager(), new DiceGroupManager(), new GameManager());
            }

            try
            {
                // DB stuff when the app opens
                using (DiceAppDbContext db = new())
                {
                    // Later, we'll use the DiceAppDbContext to get a GameDbRunner

                    // get all the players from the DB
                    PlayerDbManager playerDbManager = new(db);
                    IEnumerable<PlayerEntity> entities = await playerDbManager.GetAll();

                    foreach (PlayerEntity entity in entities)
                    {
                        try
                        {
                            // persist them  as models !
                            await masterOfCeremonies.GlobalPlayerManager.Add(entity.ToModel());
                        }
                        catch (Exception ex) { Debug.WriteLine($"{ex.Message}\n... Never mind"); }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"{ex.Message}\n... Couldn't use the database"); }

            string menuChoice = "nothing";

            while (menuChoice != "q")
            {
                Console.WriteLine(
                    "l... load a game\n" +
                    "n... start new game\n" +
                    "d... delete a game\n" +
                    "i... see all dice\n" +
                    "c... create a group of dice\n" +
                    "p... see all players\n" +
                    "y... create players\n" +
                    "q... QUIT\n" +
                    ">"
                    );

                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "q":
                        break;

                    case "l":
                        string loadName = await ChooseGame(masterOfCeremonies);
                        if (masterOfCeremonies.GameManager.GetOneByName(loadName) != null)
                        {
                            await Play(masterOfCeremonies, loadName);
                        }
                        break;

                    case "n":

                        if (!(await masterOfCeremonies.DiceGroupManager.GetAll()).Any())
                        {
                            Console.WriteLine("make at least one dice group first, then try again");
                            break;
                        }
                        Console.WriteLine("add dice to the game");
                        IEnumerable<Die> newGameDice = await PrepareDice(masterOfCeremonies);

                        string newGameName;
                        Console.WriteLine("give this new game a name\n>");
                        newGameName = Console.ReadLine();

                        Console.WriteLine("add players to the game");
                        PlayerManager playerManager = await PreparePlayers(masterOfCeremonies);

                        await masterOfCeremonies.StartNewGame(newGameName, playerManager, newGameDice);
                        await Play(masterOfCeremonies, newGameName);

                        break;

                    case "d":
                        string deleteName = await ChooseGame(masterOfCeremonies);
                        masterOfCeremonies.GameManager.Remove(await masterOfCeremonies.GameManager.GetOneByName(deleteName));
                        break;

                    case "c":
                        string newGroupName;
                        Console.WriteLine("give this new dice group a name");
                        newGroupName = Console.ReadLine();

                        List<Die> newGroupDice = new();
                        string menuChoiceNewDice = "";
                        while (!(menuChoiceNewDice.Equals("ok") && newGroupDice.Any()))
                        {
                            Die die = null;
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
                        await masterOfCeremonies.DiceGroupManager.Add(new DiceGroup(newGroupName, newGroupDice));
                        break;

                    case "p":
                        await ShowPlayers(masterOfCeremonies);
                        break;

                    case "i":
                        await ShowDice(masterOfCeremonies);
                        break;

                    case "y":
                        await PreparePlayers(masterOfCeremonies);
                        break;

                    default:
                        Console.WriteLine("u wot m8?");
                        break;
                }
            }

            try
            {
                // DB stuff when the app closes
                using (DiceAppDbContext db = new())
                {
                    // get all the players from the app's memory
                    IEnumerable<Player> models = await masterOfCeremonies.GlobalPlayerManager.GetAll();

                    // create a PlayerDbManager (and inject it with the DB)
                    PlayerDbManager playerDbManager = new(db);

                    foreach (Player model in models)
                    {
                        try // to persist them
                        { // as entities !
                            PlayerEntity entity = model.ToEntity();
                            await playerDbManager.Add(entity);
                        }
                        // what if there's already a player with that name? Exception (see PlayerEntity's annotations)
                        catch (ArgumentException ex) { Debug.WriteLine($"{ex.Message}\n... Never mind"); }
                    }
                }
                // flushing and closing NLog before quitting completely
                NLog.LogManager.Shutdown();
            }
            catch (Exception ex) { Console.WriteLine($"{ex.Message}\n... Couldn't use the database"); }
        }

        private static async Task Play(MasterOfCeremonies masterOfCeremonies, string name)
        {
            string menuChoicePlay = "";
            while (menuChoicePlay != "q")
            {
                Game game = await masterOfCeremonies.GameManager.GetOneByName(name);
                Console.WriteLine($"{PlayerToString(await game.GetWhoPlaysNow())}'s turn\n" +
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
                        foreach (Turn turn in game.GetHistory()) { Console.WriteLine(TurnToString(turn)); }
                        break;
                    case "s":
                        await masterOfCeremonies.GameManager.Add(game);
                        break;
                    default:
                        await MasterOfCeremonies.PlayGame(game);
                        Console.WriteLine(TurnToString(game.GetHistory().Last()));
                        break;
                }
            }
        }

        private static async Task<string> ChooseGame(MasterOfCeremonies masterOfCeremonies)
        {
            string name;
            Console.WriteLine("which of these games?\n(choose by name)\n>");
            foreach (Game game in await masterOfCeremonies.GameManager.GetAll())
            {
                Console.WriteLine(GameToString(game));
            }
            name = Console.ReadLine();
            return name;
        }

        private static async Task ShowPlayers(MasterOfCeremonies masterOfCeremonies)
        {
            Console.WriteLine("Look at all them players!");
            foreach (Player player in await masterOfCeremonies.GlobalPlayerManager.GetAll())
            {
                Console.WriteLine(PlayerToString(player));
            }
        }

        private static async Task ShowDice(MasterOfCeremonies masterOfCeremonies)
        {
            foreach ((string name, ReadOnlyCollection<Die> dice) in await masterOfCeremonies.DiceGroupManager.GetAll())
            {
                Console.WriteLine($"{name} -- {dice}"); // maybe code a quick and dirty DieToString()
            }
        }

        private static NumberDie MakeNumberDie()
        {
            NumberDie die;
            List<NumberFace> faces = new();
            string menuChoiceNewFaces = "";

            while (menuChoiceNewFaces != "ok")
            {
                Console.WriteLine("create a face with a number, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();

                PreventEmptyDieCreation(ref menuChoiceNewFaces, faces.Count);

                if (!menuChoiceNewFaces.Equals("ok") && int.TryParse(menuChoiceNewFaces, out int num))
                {
                    faces.Add(new(num));
                }
            }

            NumberFace[] facesArr = faces.ToArray();

            die = new NumberDie(facesArr[0], facesArr[1..]);
            return die;
        }

        private static ColorDie MakeColorDie()
        {
            ColorDie die;
            List<ColorFace> faces = new();
            string menuChoiceNewFaces = "";

            while (!menuChoiceNewFaces.Equals("ok"))
            {
                Console.WriteLine("create a face with an color name, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();

                PreventEmptyDieCreation(ref menuChoiceNewFaces, faces.Count);

                if (menuChoiceNewFaces != "ok") faces.Add(new(Color.FromName(menuChoiceNewFaces)));
            }

            ColorFace[] facesArr = faces.ToArray();

            die = new ColorDie(facesArr[0], facesArr[1..]);
            return die;
        }

        private static ImageDie MakeImageDie()
        {
            ImageDie die;
            List<ImageFace> faces = new();
            string menuChoiceNewFaces = "";

            while (!menuChoiceNewFaces.Equals("ok"))
            {
                Console.WriteLine("create a face with an image uri, or enter 'ok' if you're finished");
                menuChoiceNewFaces = Console.ReadLine();

                PreventEmptyDieCreation(ref menuChoiceNewFaces, faces.Count);

                if (menuChoiceNewFaces != "ok")
                {
                    try
                    {
                        faces.Add(new(new Uri(menuChoiceNewFaces)));
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.Warn(ex);
                    }
                    catch (UriFormatException ex)
                    {
                        Console.WriteLine("that URI was not valid");
                        Console.WriteLine(ex.Message);
                        logger.Warn(ex);
                    }
                }
            }

            ImageFace[] facesArr = faces.ToArray();

            die = new ImageDie(facesArr[0], facesArr[1..]);
            return die;
        }


        private static void PreventEmptyDieCreation(ref string menuChoice, int count)
        {
            if (menuChoice.Equals("ok") && count == 0)
            {
                Console.WriteLine("create at least one valid face");
                menuChoice = ""; // persists outside the scope of this function
            }
        }

        private async static Task<IEnumerable<Die>> PrepareDice(MasterOfCeremonies masterOfCeremonies)
        {
            List<Die> result = new();
            Console.WriteLine("all known dice or groups of dice:");
            await ShowDice(masterOfCeremonies);
            string menuChoiceDice = "";
            while (!(menuChoiceDice.Equals("ok") && result.Any()))
            {
                Console.WriteLine("write the name of a dice group you want to add (at least one), or 'ok' if you're finished");
                menuChoiceDice = Console.ReadLine();
                if (!menuChoiceDice.Equals("ok"))
                {
                    IEnumerable<Die> chosenDice = (await masterOfCeremonies.DiceGroupManager.GetOneByName(menuChoiceDice)).Dice;
                    foreach (Die die in chosenDice)
                    {
                        result.Add(die);
                    }
                }
            }
            return result.AsEnumerable();
        }
        private async static Task<PlayerManager> PreparePlayers(MasterOfCeremonies masterOfCeremonies)
        {
            PlayerManager result = new();
            Console.WriteLine("all known players:");
            await ShowPlayers(masterOfCeremonies);
            string menuChoicePlayers = "";
            while (!(menuChoicePlayers.Equals("ok") && (await result.GetAll()).Any()))
            {
                Console.WriteLine("write the name of a player you want to add (at least one), or 'ok' if you're finished");
                menuChoicePlayers = Console.ReadLine();
                if (!menuChoicePlayers.Equals("ok"))
                {
                    Player player = new(menuChoicePlayers);
                    if (!(await masterOfCeremonies.GlobalPlayerManager.GetAll()).Contains(player))
                    {
                        // if the player didn't exist, now it does... 
                        await masterOfCeremonies.GlobalPlayerManager.Add(player);
                    }
                    // almost no checks, this is temporary
                    try
                    {
                        await result.Add(player);
                    }
                    catch (ArgumentException ex) { Console.WriteLine($"{ex.Message}\n... Never mind"); }

                }
            }

            return result;
        }
        private static string TurnToString(Turn turn)
        {
            string[] datetime = turn.When.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Split("T");
            string date = datetime[0];
            string time = datetime[1];

            StringBuilder sb = new();

            sb.AppendFormat("{0} {1} -- {2} rolled:",
                date,
                time,
                PlayerToString(turn.Player));
            foreach (KeyValuePair<Die, Face> kvp in turn.DiceNFaces)
            {
                sb.Append(" " + kvp.Value.StringValue);
            }

            return sb.ToString();
        }

        private async static Task<string> GameToString(Game game)
        {
            StringBuilder sb = new();
            sb.Append($"Game: {game.Name}");

            sb.Append("\nPlayers:");
            foreach (Player player in game.PlayerManager.GetAll()?.Result)
            {
                sb.Append($" {PlayerToString(player)}");
            }

            sb.Append($"\nNext: {PlayerToString(await game.GetWhoPlaysNow())}");

            sb.Append("\nLog:\n");
            foreach (Turn turn in game.GetHistory())
            {
                sb.Append($"\t{TurnToString(turn)}\n");
            }
            return sb.ToString();
        }

        private static string PlayerToString(Player player)
        {
            return player.Name;
        }
    }
}