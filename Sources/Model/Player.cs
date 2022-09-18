using System;
using System.Xml.Linq;

namespace Model
{
    public class Player : IEquatable<Player>
    {
        public string Name
        {
            get
            {
                return name;
            }
            internal set
            {
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(""))
                {
                    name = value;
                }
                else throw new ArgumentException("player name may never be empty or null");
            }
        }
        
        private string name;

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
            return Name.ToUpper().GetHashCode();
        }
    }
}
