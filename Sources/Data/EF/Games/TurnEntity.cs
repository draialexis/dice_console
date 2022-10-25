using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Joins;
using Data.EF.Players;
using Model.Players;

namespace Data.EF.Games
{
    public sealed class TurnEntity : IEquatable<TurnEntity>
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
            if (other is null
                ||
                !(PlayerEntity.Equals(other.PlayerEntity)
                && When.Equals(other.When)
                && ID.Equals(other.ID)
                && Dice.Count == other.Dice.Count
                && Faces.Count == other.Faces.Count))
            {
                return false;
            }

            for (int i = 0; i < Faces.Count; i++)
            {
                if (Dice.ElementAt(i).Faces.Count
                    != other.Dice.ElementAt(i).Faces.Count)
                {
                    return false;
                }

                if (!other.Faces.ElementAt(i).ID
                    .Equals(Faces.ElementAt(i).ID))
                {
                    return false;
                }

                for (int j = 0; j < Dice.ElementAt(i).Faces.Count; j++)
                {
                    if (!other.Dice.ElementAt(i).Faces.ElementAt(j).ID
                        .Equals(Dice.ElementAt(i).Faces.ElementAt(j).ID))
                    {
                        return false;
                    }
                }
            }
            return true;
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