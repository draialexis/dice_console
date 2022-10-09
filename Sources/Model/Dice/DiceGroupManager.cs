using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class DiceGroupManager : IManager<KeyValuePair<string, IEnumerable<Die>>>
    {
        private readonly Dictionary<string, IEnumerable<Die>> diceGroups = new();

        public async Task<KeyValuePair<string, IEnumerable<Die>>> Add(KeyValuePair<string, IEnumerable<Die>> toAdd)
        {
            if (string.IsNullOrWhiteSpace(toAdd.Key))
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null or empty");

            }
            if (diceGroups.Contains(toAdd))
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            diceGroups.Add(toAdd.Key.Trim(), toAdd.Value);
            return await Task.FromResult(toAdd);
        }

        public async Task<IEnumerable<KeyValuePair<string, IEnumerable<Die>>>> GetAll()
        {
            return await Task.FromResult(diceGroups.AsEnumerable());
        }

        public Task<KeyValuePair<string, IEnumerable<Die>>> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public async Task<KeyValuePair<string, IEnumerable<Die>>> GetOneByName(string name)
        {
            // les groupes de dés nommés :
            // ils sont case-sensistive, mais "mon jeu" == "mon jeu " == "  mon jeu"
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "param should not be null or empty");
            }
            else
            {
                return await Task.FromResult(new KeyValuePair<string, IEnumerable<Die>>(name, diceGroups[name]));
            }
        }

        public void Remove(KeyValuePair<string, IEnumerable<Die>> toRemove)
        {
            if (toRemove.Key is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            else
            {
                diceGroups.Remove(toRemove.Key);
            }
        }

        /// <summary>
        /// updates a (string, IEnumerable-of-Die) couple. only the name can be updated
        /// </summary>
        /// <param name="before">original couple</param>
        /// <param name="after">new couple</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<KeyValuePair<string, IEnumerable<Die>>> Update(KeyValuePair<string, IEnumerable<Die>> before, KeyValuePair<string, IEnumerable<Die>> after)
        {
            // pas autorisé de changer les dés, juste le nom
            if (!before.Value.Equals(after.Value))
            {
                throw new ArgumentException("the group of dice cannot be updated, only the name", nameof(before));
            }
            if (string.IsNullOrWhiteSpace(before.Key) || string.IsNullOrWhiteSpace(after.Key))
            {
                throw new ArgumentNullException(nameof(before), "dice group name should not be null or empty");
            }
            diceGroups.Remove(before.Key);
            await Add(after);
            return after;

        }
    }
}
