using Model.Dice;
using Model.Dice.Faces;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Model.Games
{
    /// <summary>
    /// a Turn consists of a Player, a DateTime, and a IEnumerable of AbstractDieFace
    /// Like a turn in some game.
    /// </summary>
    public sealed class Turn : IEquatable<Turn>
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
        public ReadOnlyDictionary<Die, Face> DiceNFaces => new(diceNFaces);
        private readonly Dictionary<Die, Face> diceNFaces;

        /// <summary>
        /// this private constructor is to be used only by factories
        /// </summary>
        /// <param name="when">date and time of the turn</param>
        /// <param name="player">player who played the turn</param>
        /// <param name="faces">faces that were rolled</param>
        private Turn(DateTime when, Player player, IEnumerable<KeyValuePair<Die, Face>> diceNFaces)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player), "param should not be null");
            }
            if (diceNFaces is null)
            {
                throw new ArgumentNullException(nameof(diceNFaces), "param should not be null");
            }
            if (!diceNFaces.Any())
            {
                throw new ArgumentException("param should not be null", nameof(diceNFaces));
            }
            if (when.Kind != DateTimeKind.Utc)
            {
                when = when.ToUniversalTime();
            }

            When = when;
            Player = player;
            this.diceNFaces = diceNFaces.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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
        public static Turn CreateWithSpecifiedTime(DateTime when, Player player, IEnumerable<KeyValuePair<Die, Face>> diceNFaces)
        {
            return new Turn(when, player, diceNFaces);
        }

        /// <summary>
        /// creates a Turn with a default time, which is "now" in UTC. 
        /// </summary>
        /// <param name="player">player who played the turn</param>
        /// <param name="faces">faces that were rolled</param>
        /// <returns>a new Turn object</returns>
        public static Turn CreateWithDefaultTime(Player player, IEnumerable<KeyValuePair<Die, Face>> diceNFaces)
        {
            return CreateWithSpecifiedTime(DateTime.UtcNow, player, diceNFaces);
        }

        public bool Equals(Turn other)
        {
            if (other is null
                ||
                !(Player.Equals(other.Player)
                && When.Equals(other.When)
                && DiceNFaces.Count == other.DiceNFaces.Count))
            {
                return false;
            }

            // 🤮
            for (int i = 0; i < DiceNFaces.Count; i++)
            {
                if (DiceNFaces.ElementAt(i).Key.Faces.Count
                    != other.DiceNFaces.ElementAt(i).Key.Faces.Count)
                {
                    return false;
                }

                if (!other.DiceNFaces.ElementAt(i).Value.StringValue
                    .Equals(DiceNFaces.ElementAt(i).Value.StringValue))
                {
                    return false;
                }

                for (int j = 0; j < DiceNFaces.ElementAt(i).Key.Faces.Count; j++)
                {
                    if (!other.DiceNFaces.ElementAt(i).Key.Faces.ElementAt(j).StringValue
                        .Equals(DiceNFaces.ElementAt(i).Key.Faces.ElementAt(j).StringValue))
                    {
                        return false;
                    }
                }
            }
            return true;
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
            return When.GetHashCode();
        }
    }
}
