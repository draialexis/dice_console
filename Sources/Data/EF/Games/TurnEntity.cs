using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Players;

namespace Data.EF.Games
{
    public class TurnEntity
    {
        public Guid ID { get; set; }
        public DateTime When { get; set; }
        public PlayerEntity Player { get; set; }

        public List<NumberDieEntity> NumberDice { get; set; }
        public List<NumberFaceEntity> NumberFaces { get; set; }
        public List<ImageDieEntity> ImageDice { get; set; }
        public List<ImageFaceEntity> ImageFaces { get; set; }
        public List<ColorDieEntity> ColorDice { get; set; }
        public List<ColorFaceEntity> ColorFaces { get; set; }

    }
}