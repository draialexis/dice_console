using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dice
{
    public class DieManager : IManager<KeyValuePair<string, IEnumerable<AbstractDie<object>>>>
    {
        private readonly Dictionary<string, IEnumerable<AbstractDie<object>>> diceGroups = new();

        public KeyValuePair<string, IEnumerable<AbstractDie<object>>> Add(KeyValuePair<string, IEnumerable<AbstractDie<object>>> toAdd)
        {
            // on trim la clé d'abord
            diceGroups.Add(toAdd.Key.Trim(), toAdd.Value);
            return toAdd;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<AbstractDie<object>>>> GetAll()
        {
            return diceGroups.AsEnumerable();
        }

        public KeyValuePair<string, IEnumerable<AbstractDie<object>>> GetOneByName(string name)
        {
            // les groupes de dés nommés :
            // ils sont case-sensistive, mais "mon jeu" == "mon jeu " == "  mon jeu"
            return new KeyValuePair<string, IEnumerable<AbstractDie<object>>>(name, diceGroups[name]);
        }

        public void Remove(KeyValuePair<string, IEnumerable<AbstractDie<object>>> toRemove)
        {
            diceGroups.Remove(toRemove.Key);
        }

        /*public void Remove(KeyValuePair<string, IEnumerable<AbstractDie<Faces.AbstractDieFace<object>, object>>> toRemove)
        {
            throw new NotImplementedException();
        }

        public void Remove(KeyValuePair<string, IEnumerable<AbstractDie<AbstractDieFace<object>, object>>> toRemove)
        {
            throw new NotImplementedException();
        }*/

        public KeyValuePair<string, IEnumerable<AbstractDie<object>>> Update(KeyValuePair<string, IEnumerable<AbstractDie<object>>> before, KeyValuePair<string, IEnumerable<AbstractDie<object>>> after)
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

/*
        IEnumerable<KeyValuePair<string, IEnumerable<AbstractDie<Faces.AbstractDieFace<object>>>> IManager<KeyValuePair<string, IEnumerable<AbstractDie<Faces.AbstractDieFace<object>>>>>.GetAll()
        {
            throw new NotImplementedException();
        }


        KeyValuePair<string, IEnumerable<AbstractDie<Faces.AbstractDieFace<object>>>> IManager<KeyValuePair<string, IEnumerable<AbstractDie<Faces.AbstractDieFace<object>>>>>.GetOneByName(string name)
        {
            throw new NotImplementedException();
        }
*/
    }
}
