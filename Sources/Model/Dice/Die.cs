using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public abstract class Die
    {
        public IEnumerable<Face> Faces => faces;

        protected static readonly Random rnd = new();

        private readonly List<Face> faces = new();

        protected Die(Face first, params Face[] faces)
        {
            this.faces.AddRange(faces.Append(first));
        }

        public virtual Face GetRandomFace()
        {
            int faceIndex = rnd.Next(0, Faces.Count());
            return Faces.ElementAt(faceIndex);
        }
    }
}
