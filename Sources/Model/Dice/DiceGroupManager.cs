using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class DiceGroupManager : IManager<KeyValuePair<string, IEnumerable<Die>>>
    {
        private readonly Dictionary<string, IEnumerable<Die>> diceGroups = new();

        public KeyValuePair<string, IEnumerable<Die>> Add(KeyValuePair<string, IEnumerable<Die>> toAdd)
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

        /// <summary>
        /// updates a (string, IEnumerable-of-Die) couple. only the name can be updated
        /// </summary>
        /// <param name="before">original couple</param>
        /// <param name="after">new couple</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public KeyValuePair<string, IEnumerable<Die>> Update(KeyValuePair<string, IEnumerable<Die>> before, KeyValuePair<string, IEnumerable<Die>> after)
        {

            if (!before.Value.ToList().Equals(after.Value.ToList()))
            {
                throw new ArgumentException("the group of dice cannot be updated, only the name", nameof(before));
            }
            if (string.IsNullOrWhiteSpace(before.Key) || string.IsNullOrWhiteSpace(after.Key))
            {
                throw new ArgumentNullException(nameof(before), "dice group name should not be null or empty");
            }
            diceGroups.Remove(before.Key);
            diceGroups.Add(after.Key, after.Value);
            return after;

        }
    }
}
