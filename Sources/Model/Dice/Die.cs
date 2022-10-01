using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public abstract class Die
    {
        public IEnumerable<Face> ListFaces => listFaces;

        protected static readonly Random rnd = new();

        private readonly List<Face> listFaces = new();

        protected Die(params Face[] faces)
        {
            listFaces.AddRange(faces);
        }

        public Face GetRandomFace()
        {
            int faceIndex = rnd.Next(0, ListFaces.Count());
            return ListFaces.ElementAt(faceIndex);
        }
    }
}
