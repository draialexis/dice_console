using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Joins;
using Data.EF.Players;

namespace Data.EF.Games
{
    public class TurnEntity
    {
        public Guid ID { get; set; }
        public DateTime When { get; set; }
        public PlayerEntity Player { get; set; }
        public Guid PlayerEntityID { get; set; }
        public ICollection<DieEntity> Dice { get; set; } // many to many
        public List<DieTurn> DieTurns { get; set; }
        public ICollection<FaceEntity> Faces { get; set; } // many to many
        public List<FaceTurn> FaceTurns { get; set; }
    }
}