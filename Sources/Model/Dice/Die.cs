using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Model.Dice
{
    public abstract class Die:IEquatable<Die>
    {
        public ReadOnlyCollection<Face> Faces => new(faces);

        protected static readonly Random rnd = new();

        private readonly List<Face> faces = new();

        protected Die(Face first, params Face[] faces)
        {
            this.faces.AddRange(faces.Append(first));
        }

        public virtual Face GetRandomFace()
        {
            int faceIndex = rnd.Next(0, Faces.Count);
            return Faces.ElementAt(faceIndex);
        }

        public bool Equals(Die other)
        {
            return Faces == other.Faces && Faces.SequenceEqual(other.Faces);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false; // is null
            if (ReferenceEquals(obj, this)) return true; // is me
            if (!obj.GetType().Equals(GetType())) return false; // is different type
            return Equals(obj as Die); // is not me, is not null, is same type : send up
        }


    }
}
