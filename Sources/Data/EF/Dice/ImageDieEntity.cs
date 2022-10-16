using Data.EF.Dice.Faces;

namespace Data.EF.Dice
{
    public class ImageDieEntity : DieEntity
    {
        public new ICollection<ImageFaceEntity> Faces { get; set; } = new List<ImageFaceEntity>();
    }
}
