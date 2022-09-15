namespace Model
{
    public class Die
    {
        private readonly string _name;

        public Die(string name)
        {
            _name = name;
        }

        public string Name => _name;
    }
}
