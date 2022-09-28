using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Model.Dice;
using Model.Dice.Faces;
using Model.Players;

namespace Model.Games
{
    /// <summary>
    /// a Turn consists of a Player, a DateTime, and a IEnumerable of AbstractDieFace
    /// Like a turn in some game.
    /// <br/>
    /// Two turns are equal if they are litterally the same instance in RAM
    /// (default behaviors Equals() and GetHashCode())
    /// </summary>
    public class Turn
    {

        /// <summary>
        /// the date and time, adjusted to UTC
        /// </summary>
        public DateTime When { get; private set; }

        /// <summary>
        /// the Player who rolled the dice
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// the collection of Face that were rolled
        /// </summary>
        public IEnumerable<KeyValuePair<AbstractDie<AbstractDieFace>, AbstractDieFace>> DiceNFaces => diceNFaces.AsEnumerable();
        private readonly Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> diceNFaces;

        /// <summary>
        /// this private constructor is to be used only by factories
        /// </summary>
        /// <param name="when">date and time of the turn</param>
        /// <param name="player">player who played the turn</param>
        /// <param name="faces">faces that were rolled</param>
        private Turn(DateTime when, Player player, Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> diceNFaces)
        {
            When = when;
            Player = player;
            this.diceNFaces = diceNFaces;
        }

        /// <summary>
        /// creates a Turn with a specified time, passed as a parameter. 
        /// <br/>
        /// whatever the DateTimeKind of <paramref name="when"/> might be, 
        /// it will become UTC during construction
        /// </summary>
        /// <param name="when">date and time of the turn</param>
        /// <param name="player">player who played the turn</param>
        /// <param name="faces">faces that were rolled</param>
        /// <returns>a new Turn object</returns>
        public static Turn CreateWithSpecifiedTime(DateTime when, Player player, Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> diceNFaces)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player), "param should not be null");
            }
            if (diceNFaces is null)
            {
                throw new ArgumentNullException(nameof(diceNFaces), "param should not be null");
            }
            if (diceNFaces.Count == 0)
            {
                throw new ArgumentException("param should not be null", nameof(diceNFaces));
            }
            if (when.Kind != DateTimeKind.Utc)
            {
                when = when.ToUniversalTime();
            }

            return new Turn(when, player, diceNFaces);
        }

        /// <summary>
        /// creates a Turn with a default time, which is "now" in UTC. 
        /// </summary>
        /// <param name="player">player who played the turn</param>
        /// <param name="faces">faces that were rolled</param>
        /// <returns>a new Turn object</returns>
        public static Turn CreateWithDefaultTime(Player player, Dictionary<AbstractDie<AbstractDieFace>, AbstractDieFace> diceNFaces)
        {
            return CreateWithSpecifiedTime(DateTime.UtcNow, player, diceNFaces);
        }

        /// <summary>
        /// represents a turn in string format
        /// </summary>
        /// <returns>a turn in string format</returns>
        public override string ToString()
        {
            string[] datetime = When.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Split("T");
            string date = datetime[0];
            string time = datetime[1];

            StringBuilder sb = new();

            sb.AppendFormat("{0} {1} -- {2} rolled:",
                date,
                time,
                Player.ToString());
            foreach (AbstractDieFace face in this.diceNFaces.Values)
            {
                sb.Append(" " + face.ToString());
            }

            return sb.ToString();
        }

        public bool Equals(Turn other)
        {
            return Player.Equals(other.Player) 
                && When.Equals(other.When) 
                && DiceNFaces.SequenceEqual(other.DiceNFaces);

        }

        public override bool Equals(object obj)
        {
            if (obj is not Turn)
            {
                return false;
            }
            return Equals(obj as Turn);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, When, DiceNFaces);
        }
    }
}
