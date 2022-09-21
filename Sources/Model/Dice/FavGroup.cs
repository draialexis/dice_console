using System.Collections.Generic;

namespace Model.Dice
{
    public class FavGroup
    {
        private IEnumerable<Die> dice;
        public string Name { get; private set; }
    }
}
