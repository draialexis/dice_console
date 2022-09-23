using System.Collections.Generic;

namespace Model.Dice
{
    public abstract class AbstractDie<T> where T : AbstractDieFace
    {
        protected string Name;
        public IEnumerable<T> ListFaces => listFaces;

        private readonly List<T> listFaces = new();

        public AbstractDie(string name, params T[] faces)
        {
            Name = name;
            listFaces.AddRange(faces);
        }



        public string GetName() => Name;

        public abstract AbstractDieFace GetRandomFace();

        public List<T> GetDieFaces()
        {
            return (List<T>)ListFaces;
        }

    }
}
