using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class DieManager : IManager<KeyValuePair<string, IEnumerable<Die>>>
    {
        private readonly Dictionary<string, IEnumerable<Die>> diceGroups = new();

        public KeyValuePair<string, IEnumerable<Die>> Add(KeyValuePair<string, IEnumerable<Die>> toAdd)
        {
            // on trim la clé d'abord
            if (string.IsNullOrWhiteSpace(toAdd.Key))
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null or empty");

            }
            if (diceGroups.Contains(toAdd))
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            diceGroups.Add(toAdd.Key.Trim(), toAdd.Value);
            return toAdd;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<Die>>> GetAll()
        {
            return diceGroups.AsEnumerable();
        }

        public KeyValuePair<string, IEnumerable<Die>> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<string, IEnumerable<Die>> GetOneByName(string name)
        {
            // les groupes de dés nommés :
            // ils sont case-sensistive, mais "mon jeu" == "mon jeu " == "  mon jeu"
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "param should not be null or empty");
            }
            else
            {
                return new KeyValuePair<string, IEnumerable<Die>>(name, diceGroups[name]);
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

        public KeyValuePair<string, IEnumerable<Die>> Update(KeyValuePair<string, IEnumerable<Die>> before, KeyValuePair<string, IEnumerable<Die>> after)
        {
            // pas autorisé de changer les dés, juste le nom
            if (!before.Value.Equals(after.Value))
            {
                if (string.IsNullOrWhiteSpace(before.Key) || string.IsNullOrWhiteSpace(after.Key))
                {
                    throw new ArgumentNullException(nameof(before), "dice group name should not be null or empty");
                }

                diceGroups.Remove(before.Key);
                diceGroups.Add(after.Key, after.Value);
                return after;
            }
            return before;
        }
    }
}
