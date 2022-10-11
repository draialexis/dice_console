﻿using Model.Dice;
using Model.Dice.Faces;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Games
{
    public class Game
    {
        /// <summary>
        /// the name of the game 😎
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set // MasterOfCeremonies will need to take care of forbidding
                // (or allowing) having two Games with the same name etc.
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("param should not be null or blank", nameof(value));
                }
                name = value;
            }
        }
        private string name;

        /// <summary>
        /// references the position in list of the current player, for a given game.
        /// </summary>
        private int nextIndex;

        /// <summary>
        /// the turns that have been done so far
        /// </summary>
        private readonly List<Turn> turns;

        /// </summary>
        /// get a READ ONLY enumerable of all turns belonging to this game
        /// </summary>
        /// <returns>a readonly enumerable of all this game's turns</returns>
        public IEnumerable<Turn> GetHistory() => turns.AsEnumerable();

        /// <summary>
        /// the game's player manager, doing CRUD on players and switching whose turn it is
        /// </summary>
        public IManager<Player> PlayerManager { get; private set; }

        /// <summary>
        /// the group of dice used for this game
        /// </summary>
        public IEnumerable<Die> Dice => dice;
        private readonly IEnumerable<Die> dice;

        /// <summary>
        /// constructs a Game with its own history of Turns. 
        /// If <paramref name="turns"/> is null, starts a new history
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="turns">the turns that have been done so far</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, IManager<Player> playerManager, IEnumerable<Die> dice, IEnumerable<Turn> turns)
        {
            Name = name;
            PlayerManager = playerManager;
            this.turns = turns is null ? new List<Turn>() : turns.ToList();
            this.dice = dice;
            this.nextIndex = 0;
        }

        /// <summary>
        /// constructs a Game with no history of turns. 
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, IManager<Player> playerManager, IEnumerable<Die> dice)
            : this(name, playerManager, dice, null)
        { }

        /// <summary>
        /// performs a Turn, marks this Game as "started", and logs that Turn
        /// </summary>
        /// <param name="player">the player whose turn it is</param>
        public void PerformTurn(Player player)
        {
            Turn turn = Turn.CreateWithDefaultTime(
                player,
                ThrowAll()
                );
            AddTurn(turn);
        }


        private void AddTurn(Turn turn)
        {
            if (!(turns.Contains(turn)))
            {
                turns.Add(turn);
            }
        }

        /// <summary>
        /// finds and returns the player whose turn it is
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Player> GetWhoPlaysNow()
        {
            if (!(await PlayerManager.GetAll()).Any())
            {
                throw new MemberAccessException("you are exploring an empty collection\nthis should not have happened");
            }
            return (await PlayerManager.GetAll()).ElementAt(nextIndex);
        }

        /// <summary>
        /// this feels very dirty
        /// </summary>
        /// <param name="current">the current player</param>
        /// <exception cref="MemberAccessException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task PrepareNextPlayer(Player current)
        {
            IEnumerable<Player> players = await PlayerManager.GetAll();
            if (!players.Any())
            {
                throw new MemberAccessException("you are exploring an empty collection\nthis should not have happened");
            }
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current), "param should not be null");
            }
            if (!players.Contains(current))
            {
                throw new ArgumentException("param could not be found in this collection\n did you forget to add it?", nameof(current));
            }
            if (players.Last() == current)
            {
                // if we've reached the last player, we need the index to loop back around
                nextIndex = 0;
            }
            else
            {
                // else we can just move up by one from the current index
                nextIndex++;
            }
        }

        /// <summary>
        /// throws all the Dice in FavGroup and returns a list of their Faces
        /// </summary>
        /// <returns>list of AbstractDieFaces after a throw</returns>
        private Dictionary<Die, Face> ThrowAll()
        {
            Dictionary<Die, Face> faces = new();
            foreach (Die die in dice)
            {
                faces.Add(die, die.GetRandomFace());
            }
            return faces;
        }

        /// <summary>
        /// represents a Game in string format
        /// </summary>
        /// <returns>a Game in string format</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Game: {Name}");

            sb.Append("\nPlayers:");
            foreach (Player player in PlayerManager.GetAll()?.Result)
            {
                sb.Append($" {player.ToString()}");
            }

            sb.Append($"\nNext: {GetWhoPlaysNow()}");

            sb.Append("\nLog:\n");
            foreach (Turn turn in this.turns)
            {
                sb.Append($"\t{turn.ToString()}\n");
            }

            return sb.ToString();
        }
    }
}
