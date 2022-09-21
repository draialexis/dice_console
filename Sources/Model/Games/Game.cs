using Model.Dice;
using Model.Players;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Games
{
    public class Game
    {
        public string Name { get; private set; }

        public bool IsFirstTurn { get; private set; } = false;

        private readonly List<Turn> turns;

        /// </summary>
        /// get a READ ONLY enumerable of all turns belonging to this game
        /// </summary>
        /// <returns>a readonly enumerable of all this game's turns</returns>
        public IEnumerable<Turn> GetHistory() => turns.AsEnumerable();

        private readonly PlayerManager playerManager;

        private readonly FavGroup favGroup;

        public Game(string name, IEnumerable<Turn> turns, PlayerManager playerManager, FavGroup favGroup)
        {
            Name = name;
            this.turns = turns.ToList() ?? new List<Turn>();
            this.playerManager = playerManager ?? new PlayerManager();
            this.favGroup = favGroup ?? new FavGroup();
        }

        public Game(string name)
            : this(name, null, null, null)
        { }

        public void PerformTurn()
        {
            Player player = playerManager.WhoPlaysNow(IsFirstTurn);
            if (IsFirstTurn) { IsFirstTurn = false; } // only true one time (on the first turn...)


            // throw favGroup's dice and record it in a "faces" var

            Turn turn = Turn.CreateWithDefaultTime(new Player(player)/*, faces*/); //using a copy so that next line doesn't "change history"
            playerManager.PrepareNextPlayer(player);
            turns.Add(turn);
        }

        // TODO test and debug
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
