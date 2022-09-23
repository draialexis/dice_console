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
        /// whether this game is new or not
        /// </summary>
        public bool IsFirstTurn { get; private set; } = false;

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
        private readonly PlayerManager playerManager;

        /// <summary>
        /// the group of dice used for this game
        /// </summary>
        private readonly FavGroup favGroup;

        /// <summary>
        /// constructs a Game with its own history of Turns. 
        /// If <paramref name="turns"/> is null, starts a new history
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="turns">the turns that have been done so far</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, PlayerManager playerManager, FavGroup favGroup, IEnumerable<Turn> turns)
        {
            Name = name;
            this.turns = turns.ToList() ?? new List<Turn>();
            this.playerManager = playerManager;
            this.favGroup = favGroup;
        }

        /// <summary>
        /// constructs a Game with no history of turns. 
        /// </summary>
        /// <param name="name">the name of the game 😎</param>
        /// <param name="playerManager">the game's player manager, doing CRUD on players and switching whose turn it is</param>
        /// <param name="favGroup">the group of dice used for this game</param>
        public Game(string name, PlayerManager playerManager, FavGroup favGroup)
            : this(name, playerManager, favGroup, null)
        { }

        /// <summary>
        /// performs a Turn, marks this Game as "started", and logs that Turn
        /// </summary>
        /// <param name="player">the player whose turn it is</param>
        public void PerformTurn(Player player)
        {
            if (IsFirstTurn) { IsFirstTurn = false; } // only true one time (on the first turn...)
            Turn turn = Turn.CreateWithDefaultTime(
                new Player(player),
                ThrowAll() //using a copy so that next line doesn't "change history"
                );
            turns.Add(turn);
        }

        /// <summary>
        /// finds whose turn it is
        /// </summary>
        /// <returns>the Player whose turn it is</returns>
        public Player GetWhoPlaysNow()
        {
            return playerManager.WhoPlaysNow(IsFirstTurn);
        }

        /// <summary>
        /// throws all the Dice in FavGroup and returns a list of their Faces
        /// </summary>
        /// <returns>list of AbstractDieFaces after a throw</returns>
        private List<AbstractDieFace> ThrowAll()
        {
            List<AbstractDieFace> faces = new();
            foreach (Die die in favGroup.Dice)
            {
                faces.Add(die.Throw());
            }
            return faces;
        }

        /// <summary>
        /// asks the PlayerManager to prepare the next Player
        /// </summary>
        /// <param name="currentPlayer">the Player whose turn it was</param>
        public void PrepareNextPlayer(Player currentPlayer)
        {
            playerManager.PrepareNextPlayer(currentPlayer);
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
                playerManager.WhoPlaysNow(IsFirstTurn));

            foreach (Turn turn in this.turns)
            {
                sb.Append("\t" + turn.ToString());
            }

            return sb.ToString();
        }
    }
}
