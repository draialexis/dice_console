using System;

namespace Model.Players
{
    /// <summary>
    /// A player for the purpose of a game of dice, consists of a name
    /// <br/>
    /// Two players are equal for Equals() and GetHashCode() if they have the same name, once trimmed and case-insensitive
    /// </summary>
    public sealed class Player : IEquatable<Player>
    {
        /// <summary>
        /// a player's unique username
        /// </summary>
        public string Name { get; private set; }
        public Player(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name.Trim();
            }
            else throw new ArgumentException("param should not be null or blank", nameof(name));
        }

        /// <summary>
        /// this is a copy contructor
        /// </summary>
        /// <param name="player">a player object to be copied</param>
        public Player(Player player)
            : this(player?.Name) // passes the player's name if player exists, else null
        { }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Player other)
        {
            return other is not null && Name.ToUpper() == other.Name.ToUpper(); // equality is case insensitive
        }

        public override bool Equals(object obj)
        {
            if (obj is not Player)
            {
                return false;
            }
            return Equals(obj as Player); // casting, to send it to the above Equals() method
        }

        public override int GetHashCode()
        {
            return Name.ToUpper().GetHashCode(); // hash is case insensitive
        }
    }
}
