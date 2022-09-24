using Model.Dice.Faces;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class DieManager : IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>>
    {
        private readonly Dictionary<string, IEnumerable<AbstractDie<AbstractDieFace>>> diceGroups = new();

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> Add(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> toAdd)
        {
            diceGroups.Add(toAdd.Key, toAdd.Value);
            return toAdd;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> GetAll()
        {
            return diceGroups.AsEnumerable();
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> GetOneByName(string name)
        {
            return new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(name, diceGroups[name]);
        }

        public void Remove(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> toRemove)
        {
            diceGroups.Remove(toRemove.Key);
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> Update(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> before, KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> after)
        {
            // check if key 1 exists
            // check if both keys same
            diceGroups.Remove(before.Key);
            diceGroups.Add(after.Key, after.Value);
            return after;
        }
    }
}
