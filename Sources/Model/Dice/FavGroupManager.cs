using System.Collections.Generic;

namespace Model.Dice
{
    public class FavGroupManager
    {
        private IEnumerable<FavGroup> favGroups;

        private readonly DieManager dieManager;

        public FavGroupManager(DieManager dieManager)
        {
            this.dieManager = dieManager;
        }
    }
}
