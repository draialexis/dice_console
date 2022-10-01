using System;
using System.Collections.Generic;
using System.Linq;
using Model.Dice.Faces;

namespace Model.Dice
{
    public abstract class AbstractDie<T> : RandomnessHaver 
    {
        public IEnumerable<AbstractDieFace<T>> ListFaces => listFaces;

        private readonly List<AbstractDieFace<T>> listFaces = new();

        protected AbstractDie(params AbstractDieFace<T>[] faces)
        {
            listFaces.AddRange(faces);
        }

        public AbstractDieFace<T> GetRandomFace()
        {
            int faceIndex = rnd.Next(0, ListFaces.Count());
            return ListFaces.ElementAt(faceIndex);
        }
    }
}
