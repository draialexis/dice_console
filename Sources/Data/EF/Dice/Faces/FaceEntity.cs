using Data.EF.Games;
using Data.EF.Joins;
using System.Diagnostics.CodeAnalysis;

namespace Data.EF.Dice.Faces
{
    /// <summary>
    /// not designed to be instantiated, but not abstract in order to allow extensions
    /// </summary>
    public class FaceEntity : IEqualityComparer<FaceEntity>
    {
        public Guid ID { get; set; }
        public ICollection<TurnEntity> Turns { get; set; } // many to many
        public List<FaceTurn> FaceTurns { get; set; }

        public bool Equals(FaceEntity x, FaceEntity y)
        {
            return x is not null
                && y is not null
                && x.ID.Equals(y.ID);
        }

        public int GetHashCode([DisallowNull] FaceEntity obj)
        {
            return ID.GetHashCode();
        }
    }
}
