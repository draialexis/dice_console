using Data.EF.Games;
using Data.EF.Joins;

namespace Data.EF.Dice.Faces
{
    /// <summary>
    /// not designed to be instantiated, but not abstract in order to allow extensions
    /// </summary>
    public class FaceEntity
    {
        public Guid ID { get; set; }
        public ICollection<TurnEntity> Turns { get; set; } // many to many
        public List<FaceTurn> FaceTurns { get; set; }
    }
}
