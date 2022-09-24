using System;
using System.Collections.Generic;
using System.Linq;
using Model.Dice.Faces;

namespace Model.Dice
{
    public abstract class AbstractDie<T> where T : AbstractDieFace
    {
        protected string Name;
        public IEnumerable<T> ListFaces => listFaces;

        private readonly List<T> listFaces = new();

        private readonly Random rnd = new();

        protected AbstractDie(string name, params T[] faces)
        {
            Name = name;
            listFaces.AddRange(faces);
        }



        public string GetName() => Name;

        public T GetRandomFace()
        {
            int faceIndex = rnd.Next(1, ListFaces.Count() + 1);
            return ListFaces.ElementAt(faceIndex);
        }

        public List<T> GetDieFaces()
        {
            return (List<T>)ListFaces;
        }

    }
}
