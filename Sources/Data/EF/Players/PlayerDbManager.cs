using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

namespace Data.EF.Players
{
    public sealed class PlayerDbManager : IManager<PlayerEntity>
    {
        private readonly DiceAppDbContext db;
        public PlayerDbManager(DiceAppDbContext db)
        {
            this.db = db;
        }

        public PlayerEntity Add(PlayerEntity toAdd)
        {
            if (db.Players!.Where(entity => entity.Name == toAdd.Name).Any())
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            EntityEntry ee = db.Players!.Add(toAdd);
            db.SaveChanges();
            return (PlayerEntity)ee.Entity;
        }


        public IEnumerable<PlayerEntity> GetAll()
        {
            return db.Players!.AsEnumerable();
        }

        public PlayerEntity GetOneByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(PlayerEntity toRemove)
        {
            throw new NotImplementedException();
        }

        public PlayerEntity Update(PlayerEntity before, PlayerEntity after)
        {
            throw new NotImplementedException();
        }

        public PlayerEntity GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }
    }
}
