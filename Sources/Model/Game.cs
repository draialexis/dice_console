using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
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

        public PlayerManager PlayerManager { get; }

        public Game(string name, IEnumerable<Turn> turns, PlayerManager playerManager)
        {
            Name = name;
            this.turns = turns.ToList() ?? new List<Turn>();
            PlayerManager = playerManager ?? new PlayerManager();
        }

        public Game(string name)
            : this(name, null, null)
        { }

        public void PerformTurn()
        {
            Player player = PlayerManager.WhoPlaysNow(IsFirstTurn);
            if (IsFirstTurn) { IsFirstTurn = false; } // only true one time (on the first turn...)


            // in a "faces" var, throw all the dice and stuff...

            Turn turn = Turn.CreateWithDefaultTime(new Player(player)/*, faces*/); //using a copy so that next line doesn't "change history"
            PlayerManager.PrepareNextPlayer(player);
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
                PlayerManager.GetAll().ToString(),
                PlayerManager.WhoPlaysNow(IsFirstTurn));

            foreach (Turn turn in this.turns)
            {
                sb.Append("\t" + turn.ToString());
            }

            return sb.ToString();
        }
    }
}
