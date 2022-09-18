using System;

namespace Model
{
    public class Player : IEquatable<Player>
    {
        public string Name { get; internal set; }

        public Player(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Player other)
        {
            return Name.ToUpper() == other.Name.ToUpper();
        }

        public override bool Equals(Object obj)
        {
            if (obj is not Player)
            {
                return false;
            }
            return Equals(obj as Player);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
