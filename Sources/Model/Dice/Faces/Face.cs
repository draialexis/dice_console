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

        public bool Equals(Face<T> other)
        {
            return Value.Equals(other.Value);  
        }

        
         public override bool Equals(object obj)
        {
            if (obj is null) return false; // is null
            if (ReferenceEquals(obj, this)) return true; // is me
            if (!obj.GetType().Equals(GetType())) return false; // is different type
            return Equals(obj as DiceGroup); // is not me, is not null, is same type : send up
        }
    }


}
