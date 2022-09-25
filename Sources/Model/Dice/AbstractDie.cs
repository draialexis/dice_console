using System;
using System.Collections.Generic;
using System.Linq;
using Model.Dice.Faces;

namespace Model.Dice
{
    public abstract class AbstractDie<T> : RandomnessHaver where T : AbstractDieFace
    {
        public IEnumerable<T> ListFaces => listFaces;

        private readonly List<T> listFaces = new();

        protected AbstractDie(params T[] faces)
        {
            listFaces.AddRange(faces);
        }

        public T GetRandomFace()
        {
            int faceIndex = rnd.Next(0, ListFaces.Count());
            return ListFaces.ElementAt(faceIndex);
        }
    }
}
