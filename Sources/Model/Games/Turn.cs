using System;
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
        public readonly DateTime when;

        /// <summary>
        /// the Player who rolled the dice
        /// </summary>
        public readonly Player player;

        // ... faces


        /// <summary>
        /// this private constructor is to be used only by factories
        /// </summary>
        /// <param name="when">date and time of the turn</param>
        /// <param name="player">player who played the turn</param>

        // ... faces
        private Turn(DateTime when, Player player/*, IEnumerable<AbstractDieFace> faces*/)
        {
            this.when = when;
            this.player = player;
            // ... faces

        }

        /// <summary>
        /// creates a Turn with a specified time, passed as a parameter. 
        /// <br/>
        /// whatever the DateTimeKind of <paramref name="when"/> might be, 
        /// it will become UTC during construction
        /// </summary>
        /// <param name="when">date and time of the turn</param>
        /// <param name="player">player who played the turn</param>
        /// <returns>a new Turn object</returns>

        // ... faces
        public static Turn CreateWithSpecifiedTime(DateTime when, Player player/*, IEnumerable<AbstractDieFace> faces*/)
        {

            // ... faces
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), "param should not be null");
            }
            if (when.Kind != DateTimeKind.Utc)
            {
                when = when.ToUniversalTime();
            }

            return new Turn(when, player/*, faces*/);
        }

        /// <summary>
        /// creates a Turn with a default time, which is "now" in UTC. 
        /// </summary>
        /// <param name="player">player who played the turn</param>
        /// <returns>a new Turn object</returns>

        // ... faces
        public static Turn CreateWithDefaultTime(Player player/*, IEnumerable<AbstractDieFace> faces*/)
        {
            return CreateWithSpecifiedTime(DateTime.UtcNow, player/*, faces*/);
        }

        // ... faces

        /// <summary>
        /// represents a turn in string format
        /// </summary>
        /// <returns>a turn in string format</returns>
        public override string ToString()
        {
            return string.Format("{0} -- {1} rolled {2}",
                ToStringIsoWithZ(),
                player.ToString(),
                "<face>, <face>, <face>...");
        }

        private string ToStringIsoWithZ()
        {
            return when.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z";
        }
    }
}
