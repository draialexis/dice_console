namespace Model.Dice.Faces
{
    public abstract class Face
    {
        public string StringValue { get; protected set; }
    }

    public abstract class Face<T> : Face
    {
        public T Value { get; protected set; }

        protected Face(T value)
        {
            Value = value;
            StringValue = value.ToString();
        }
    }
}
