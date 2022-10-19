using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Model.Dice
{
    public sealed class DiceGroup : IEquatable<DiceGroup>
    {
        public string Name { get; private set; }
        public ReadOnlyCollection<Die> Dice => new(dice);

        private readonly List<Die> dice = new();

        public DiceGroup(string name, IEnumerable<Die> dice)
        {
            Name = name;
            this.dice.AddRange(dice);
        }

        public bool Equals(DiceGroup other)
        {
            return Name == other.Name && Dice.SequenceEqual(other.Dice);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false; // is null
            if (ReferenceEquals(obj, this)) return true; // is me
            if (!obj.GetType().Equals(GetType())) return false; // is different type
            return Equals(obj as DiceGroup); // is not me, is not null, is same type : send up
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, dice);
        }

        public void Deconstruct(out string name, out ReadOnlyCollection<Die> dice)
        {
            dice = Dice;
            name = Name;
        }
    }
}
