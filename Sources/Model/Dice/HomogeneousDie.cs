using Model.Dice.Faces;

namespace Model.Dice
{
    public abstract class HomogeneousDie<T> : Die
    {
        protected HomogeneousDie(params Face<T>[] faces) : base(faces)
        {
        }
    }
}
