using Model.Players;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class FavGroupManager
    {
        private readonly List<FavGroup> favGroups = new();

        private readonly DieManager dieManager;

        public FavGroupManager(DieManager dieManager)
        {
            this.dieManager = dieManager;
        }

        public IEnumerable<FavGroup> GetAll() => favGroups.AsEnumerable();

    }
}
