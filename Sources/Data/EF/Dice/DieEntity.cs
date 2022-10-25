using Data.EF.Dice.Faces;
using Data.EF.Games;
using Data.EF.Joins;
using System.Diagnostics.CodeAnalysis;

namespace Data.EF.Dice
{
    /// <summary>
    /// not designed to be instantiated, but not abstract in order to allow extensions
    /// </summary>
    /// 
    public class DieEntity : IEqualityComparer<DieEntity>
    {
        public Guid ID { get; set; }
        public ICollection<FaceEntity> Faces { get; set; } = new List<FaceEntity>(); // one to many
        public ICollection<TurnEntity> Turns { get; set; } = new List<TurnEntity>(); // many to many
        public List<DieTurn> DieTurns { get; set; } = new();

        public bool Equals(DieEntity x, DieEntity y)
        {
            return x is not null
                && y is not null
                && x.ID.Equals(y.ID)
                && x.Faces.Equals(y.Faces);
        }

        public int GetHashCode([DisallowNull] DieEntity obj)
        {
            return HashCode.Combine(ID, Faces);
        }
    }
}
