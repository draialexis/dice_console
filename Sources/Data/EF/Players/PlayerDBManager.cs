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
            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");
            }
            if (string.IsNullOrWhiteSpace(toAdd.Name))
            {
                throw new ArgumentException("Name property should not be null or whitespace", nameof(toAdd));
            }
            EntityEntry ee = db.Players!.Add(toAdd);
            db.SaveChanges();
            return (PlayerEntity)ee.Entity;
        }

        public IEnumerable<PlayerEntity> GetAll()
        {
            return db.Players!.AsEnumerable();
        }

        /// <summary>
        /// Calls First(), which will throw an exception if no player with such name exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PlayerEntity GetOneByName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name property should not be null or whitespace", nameof(name));
            }
            return db.Players!.First(p => p.Name == name);
        }

        public void Remove(PlayerEntity toRemove)
        {
            if (toRemove is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            db.Players!.Remove(toRemove);
            db.SaveChanges();
        }

        public PlayerEntity Update(PlayerEntity before, PlayerEntity after)
        {
            EntityEntry ee = db.Players!.Update(before);
            before.Name = after.Name;
            db.SaveChanges();
            return (PlayerEntity)ee.Entity;

        }

        /// <summary>
        /// Calls First(), which will throw an exception if no player with such ID exists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PlayerEntity GetOneByID(Guid ID)
        {
           return db.Players!.First(p => p.ID == ID);
        }
    }
}
