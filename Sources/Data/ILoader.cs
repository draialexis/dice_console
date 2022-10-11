using Model.Games;

namespace Data
{
    public interface ILoader
    {
        public Task<MasterOfCeremonies> LoadApp();
    }
}
