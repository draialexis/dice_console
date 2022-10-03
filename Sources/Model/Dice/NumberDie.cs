using Model.Dice.Faces;

namespace Model.Dice
{
    public class NumberDie : HomogeneousDie<int>
    {
        public NumberDie(params NumberFace[] faces) : base(faces)
        {
        }
    }
}
