using Model.Players;
using System.Collections.Generic;

namespace Model.Dice
{
    public class FavGroup
    {
        public IEnumerable<Die> Dice { get; private set; }

        public string Name { get; private set; }

    }
}
