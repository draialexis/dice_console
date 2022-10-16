using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Games;

namespace Data.EF.Joins
{
    public class DieTurn
    {
        public Guid DieEntityID { get; set; }
        public DieEntity DieEntity { get; set; }

        public Guid TurnEntityID { get; set; }
        public TurnEntity TurnEntity { get; set; }
    }
}
