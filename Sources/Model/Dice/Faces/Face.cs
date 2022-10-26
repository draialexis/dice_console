using System;

namespace Model.Dice.Faces
{
    public abstract class Face 
    {
        public string StringValue { get; protected set; }


    }

    public abstract class Face<T> : Face, IEquatable<Face<T>>
    {
        public T Value { get; protected set; }

        protected Face(T value)
        {
            Value = value;
            StringValue = value.ToString();
        }

        public bool Equals(DiceGroup other)
        {
            return StringValue == other.Name;
        }


    }


}
