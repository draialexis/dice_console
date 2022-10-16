using Data.EF.Dice.Faces;

namespace Data.EF.Dice
{
    public class ColorDieEntity : DieEntity
    {
        public new ICollection<ColorFaceEntity> Faces { get; set; }
    }
}
