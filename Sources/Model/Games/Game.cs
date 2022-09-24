using Model.Dice;
using Model.Dice.Faces;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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
            set // GameRunner will need to take care of forbidding
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
        private int nextIndex = 0;

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
        private readonly IManager<Player> playerManager;

        /// <summary>
        /// the group of dice used for this game
        /// </summary>
        private readonly IEnumerable<AbstractDie<AbstractDieFace>> dice;

        /// <summary>
        /// constructs a Game with its own history of Turns. 
        /// If <paramref name="turns"/> is null, starts a new history
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="turns">the turns that have been done so far</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, IManager<Player> playerManager, IEnumerable<AbstractDie<AbstractDieFace>> dice, IEnumerable<Turn> turns)
        {
            Name = name;
            this.turns = turns.ToList() ?? new List<Turn>();
            this.playerManager = playerManager;
            this.dice = dice;
        }

        /// <summary>
        /// constructs a Game with no history of turns. 
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, IManager<Player> playerManager, IEnumerable<AbstractDie<AbstractDieFace>> dice)
            : this(name, playerManager, dice, null)
        { }

        /// <summary>
        /// performs a Turn, marks this Game as "started", and logs that Turn
        /// </summary>
        /// <param name="player">the player whose turn it is</param>
        public void PerformTurn(Player player)
        {
            Turn turn = Turn.CreateWithDefaultTime(
                new Player(player),
                ThrowAll() //using a copy so that next lines can't "change history"
                );
            turns.Add(turn);
            nextIndex++;
        }

        /// <summary>
        /// finds and returns the player whose turn it is
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Player GetWhoPlaysNow()
        {
            if (!playerManager.GetAll().Any())
            {
                throw new MemberAccessException("you are exploring an empty collection\nthis should not have happened");
            }

            return playerManager.GetAll().ElementAt(nextIndex);
        }

        /// <summary>
        /// this feels very dirty
        /// </summary>
        /// <param name="current">the current player</param>
        /// <exception cref="MemberAccessException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void PrepareNextPlayer(Player current)
        {
            if (!playerManager.GetAll().Any())
            {
                throw new MemberAccessException("you are exploring an empty collection\nthis should not have happened");
            }
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current), "param should not be null");
            }
            if (!playerManager.GetAll().Contains(current))
            {
                throw new ArgumentException("param could not be found in this collection\n did you forget to add it?", nameof(current));
            }

            if (playerManager.GetAll().Last() == current)
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
        private List<AbstractDieFace> ThrowAll()
        {
            List<AbstractDieFace> faces = new();
            foreach (AbstractDie<AbstractDieFace> die in dice)
            {
                faces.Add(die.GetRandomFace());
            }
            return faces;
        }

        public Player AddPlayerToGame(Player player)
        {
            return playerManager.Add(player);
        }
        
        public IEnumerable<Player> GetPlayersFromGame()
        {
            return playerManager.GetAll();
        }
        
        public Player UpdatePlayerInGame(Player oldPlayer, Player newPlayer)
        {
            return playerManager.Update(oldPlayer, newPlayer);
        }

        public void RemovePlayerFromGame(Player player)
        {
            playerManager.Remove(player);
        }
        
        /// <summary>
        /// represents a Game in string format
        /// </summary>
        /// <returns>a Game in string format</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendFormat("Game: {0}===========\n" +
                "{1} are playing. {2} is next.\n" +
                "Log:\n",
                Name,
                playerManager.GetAll().ToString(),
                GetWhoPlaysNow());

            foreach (Turn turn in this.turns)
            {
                sb.Append("\t" + turn.ToString());
            }

            return sb.ToString();
        }
    }
}
