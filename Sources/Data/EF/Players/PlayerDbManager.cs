using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

namespace Data.EF.Players
{
    public sealed class PlayerDbManager : IManager<PlayerEntity>
    {
        private readonly DiceAppDbContext db;

        public PlayerDbManager(DiceAppDbContext db)
        {
            if (db is null)
            {
                throw new ArgumentNullException(nameof(db), "param should not be null");
            }
            this.db = db;
        }

        /// <summary>
        /// side effect: entity's name is trimmed.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static void CleanPlayerEntity(PlayerEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "param should not be null");
            }
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ArgumentException("Name property should not be null or whitespace", nameof(entity));
            }
            entity.Name = entity.Name.Trim();
        }

        /// <summary>
        /// adds a non-null PlayerEntity with a valid name to this mgr's context
        /// </summary>
        /// <param name="toAdd">the entity to add</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<PlayerEntity> Add(PlayerEntity toAdd)
        {
            CleanPlayerEntity(toAdd);

            if (db.PlayerEntity.Where(entity => entity.Name == toAdd.Name).Any())
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }

            return InternalAdd(toAdd);
        }

        private async Task<PlayerEntity> InternalAdd(PlayerEntity toAdd)
        {
            EntityEntry ee = await db.PlayerEntity.AddAsync(toAdd);
            await db.SaveChangesAsync();
            return (PlayerEntity)ee.Entity;
        }

        public async Task<IEnumerable<PlayerEntity>> GetAll()
        {
            List<PlayerEntity> players = new();
            await Task.Run(() => players.AddRange(db.PlayerEntity));
            return players.AsEnumerable();
        }

        /// <summary>
        /// This will throw an exception if no player with such name exists. 
        /// If you want to know whether any player with that name exists, call IsPresentByName()
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public Task<PlayerEntity> GetOneByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name property should not be null or whitespace", nameof(name));
            }
            name = name.Trim();
            return InternalGetOneByName(name);
        }


        private async Task<PlayerEntity> InternalGetOneByName(string name)
        {
            return await db.PlayerEntity.Where(p => p.Name == name).FirstAsync();
        }

        public async Task<bool> IsPresentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            name = name.Trim();
            return await db.PlayerEntity.Where(p => p.Name == name).FirstOrDefaultAsync() is not null;
        }

        /// <summary>
        /// removes a non-null PlayerEntity with a valid name from this mgr's context
        /// </summary>
        /// <param name="toRemove">the entity to remove</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(PlayerEntity toRemove)
        {
            CleanPlayerEntity(toRemove);
            bool isPresent = IsPresentByID(toRemove.ID).Result;
            if (isPresent)
            {
                db.PlayerEntity.Remove(toRemove);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// updates a non-null PlayerEntity with a valid name in this mgr's context. This cannot update an ID
        /// </summary>
        /// <param name="before">the entity to update</param>
        /// <param name="after">the entity to replace 'before'</param>
        /// <returns>the updated entity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<PlayerEntity> Update(PlayerEntity before, PlayerEntity after)
        {
            PlayerEntity[] args = { before, after };

            foreach (PlayerEntity entity in args)
            {
                CleanPlayerEntity(entity);
            }

            if (before.ID != after.ID)
            {
                throw new ArgumentException("ID cannot be updated", nameof(after));
            }

            return InternalUpdate(before, after);

        }

        private async Task<PlayerEntity> InternalUpdate(PlayerEntity before, PlayerEntity after)
        {
            Remove(before);
            return await Add(after);
        }

        /// <summary>
        /// This will throw an exception if no player with such ID exists. 
        /// If you want to know whether any player with that ID exists, call IsPresentByID()
        /// </summary>
        /// <param name="ID">the ID to look for</param>
        /// <returns>PlayerEntity with that ID</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<PlayerEntity> GetOneByID(Guid ID)
        {
            return await db.PlayerEntity.FirstAsync(p => p.ID == ID);
        }

        public async Task<bool> IsPresentByID(Guid ID)
        {
            return await db.PlayerEntity.FirstOrDefaultAsync(p => p.ID == ID) is not null;
        }
    }
}
