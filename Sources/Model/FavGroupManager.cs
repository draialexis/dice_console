using System.Collections.Generic;

namespace Model
{
    public class FavGroupManager
    {
        private IEnumerable<FavGroup> favGroups;

        private DieManager dieManager;

        public FavGroupManager(DieManager dieManager)
        {
            this.dieManager = dieManager;
        }
    }
}
