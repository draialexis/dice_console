using Data.EF.Dice.Faces;

namespace Data.EF.Dice
{
    public class NumberDieEntity : DieEntity
    {
        public new ICollection<NumberFaceEntity> Faces { get; set; } = new List<NumberFaceEntity>();
    }
}
