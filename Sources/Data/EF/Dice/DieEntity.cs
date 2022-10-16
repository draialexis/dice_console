using Data.EF.Dice.Faces;
using Data.EF.Games;
using Data.EF.Joins;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Dice
{
    /// <summary>
    /// not designed to be instantiated, but not abstract in order to allow extensions
    /// </summary>
    /// 
    public class DieEntity
    {
        public Guid ID { get; set; }
        public ICollection<TurnEntity> Turns { get; set; } // many to many
        public List<DieTurn> DieTurns { get; set; }
        public ICollection<FaceEntity> Faces { get; set; } // one to many
    }
}
