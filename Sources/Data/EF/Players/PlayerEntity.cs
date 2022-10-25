using Data.EF.Games;
using Microsoft.EntityFrameworkCore;

namespace Data.EF.Players
{
    [Index(nameof(Name), IsUnique = true)]
    public sealed class PlayerEntity : IEquatable<PlayerEntity>
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public ICollection<TurnEntity> Turns { get; set; } = new List<TurnEntity>();

        public override bool Equals(object obj)
        {
            if (obj is not PlayerEntity)
            {
                return false;
            }
            return Equals(obj as PlayerEntity);
        }

        public bool Equals(PlayerEntity other)
        {
            return other is not null && this.ID.Equals(other.ID) && this.Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Name);
        }

        public override string ToString()
        {
            return $"{ID.ToString().ToUpper()} -- {Name}";
        }


    }
}
