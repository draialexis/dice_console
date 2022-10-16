using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Joins;
using Data.EF.Players;

namespace Data.EF.Games
{
    public sealed class TurnEntity: IEquatable<TurnEntity>
    {
        public Guid ID { get; set; }
        public DateTime When { get; set; }
        public PlayerEntity PlayerEntity { get; set; }
        public Guid PlayerEntityID { get; set; }
        public ICollection<DieEntity> Dice { get; set; } = new List<DieEntity>(); // many to many
        public List<DieTurn> DieTurns { get; set; } = new();
        public ICollection<FaceEntity> Faces { get; set; } = new List<FaceEntity>(); // many to many
        public List<FaceTurn> FaceTurns { get; set; } = new();

        public override bool Equals(object obj)
        {
            if (obj is not TurnEntity)
            {
                return false;
            }
            return Equals(obj as TurnEntity);
        }

        public bool Equals(TurnEntity other)
        {
            return other is not null
                && this.ID.Equals(other.ID)
                && this.When.Equals(other.When)
                && this.PlayerEntity.Equals(other.PlayerEntity)
                && this.Dice.SequenceEqual(other.Dice)
                && this.Faces.SequenceEqual(other.Faces);
        }

        public override int GetHashCode()
        {
            int result = HashCode.Combine(
                ID,
                When,
                PlayerEntity);

            foreach (DieEntity die in Dice)
            {
                result += die.GetHashCode();
            }

            foreach (FaceEntity face in Faces)
            {
                result += face.GetHashCode();
            }

            return result;
        }
    }
}