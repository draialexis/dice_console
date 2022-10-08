using Model.Dice.Faces;

namespace Model.Dice
{
    public class NumberDie : HomogeneousDie<int>
    {
        public NumberDie(NumberFace first, params NumberFace[] faces) 
            : base(first, faces)
        {
        }
    }
}
