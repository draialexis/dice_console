using Model.Dice.Faces;

namespace Model.Dice
{
    public abstract class HomogeneousDie<T> : Die
    {
        protected HomogeneousDie(Face<T> first, params Face<T>[] faces) 
            : base(first, faces)
        {
        }

        public override Face<T> GetRandomFace()
        {
            return (Face<T>)base.GetRandomFace();
        }
    }
}
