using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class DieManager : IManager<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>>
    {
        private readonly Dictionary<string, IEnumerable<AbstractDie<AbstractDieFace>>> diceGroups = new();

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> Add(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> toAdd)
        {
            // on trim la clé d'abord
            if (toAdd.Key == null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");

            }
            if (diceGroups.Contains(toAdd))
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            diceGroups.Add(toAdd.Key.Trim(), toAdd.Value);
            return toAdd;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>> GetAll()
        {
            return diceGroups.AsEnumerable();
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> GetOneByName(string name)
        {
            // les groupes de dés nommés :
            // ils sont case-sensistive, mais "mon jeu" == "mon jeu " == "  mon jeu"
            if (name != null)
            {
                return new KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>>(name, diceGroups[name]);
            }
            else {
                throw new ArgumentNullException(nameof(name), "param should not be null");


            }
        }

        public void Remove(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> toRemove)
        {
            if (toRemove.Key != null)
            { 
                throw new ArgumentNullException(nameof(toRemove), "param should not be null"); }
            else
            {
                diceGroups.Remove(toRemove.Key);
            }


        }

        public KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> Update(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> before, KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace>>> after)
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
