using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class DiceGroupManager : IManager<DiceGroup>
    {
        private readonly List<DiceGroup> diceGroups = new();

        public Task<DiceGroup> Add(DiceGroup toAdd)
        {
            if (string.IsNullOrWhiteSpace(toAdd.Name))
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null or empty");

            }
            if (diceGroups.Contains(toAdd))
            {
                throw new ArgumentException("this dice group already exists", nameof(toAdd));
            }
            diceGroups.Add(new(toAdd.Name.Trim(), toAdd.Dice));
            return Task.FromResult(toAdd);
        }

        public Task<DiceGroup> AddCheckName(DiceGroup toAdd)
        {
            if (string.IsNullOrWhiteSpace(toAdd.Name))
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null or empty");

            }
            if (diceGroups.Where(d => d.Name.Equals(toAdd.Name)).Any())
            {
                throw new ArgumentException("this dice group already exists", nameof(toAdd));
            }
            diceGroups.Add(new(toAdd.Name.Trim(), toAdd.Dice));
            return Task.FromResult(toAdd);
        }

        public Task<ReadOnlyCollection<DiceGroup>> GetAll()
        {
            return Task.FromResult(new ReadOnlyCollection<DiceGroup>(diceGroups));
        }

        public Task<DiceGroup> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public Task<DiceGroup> GetOneByName(string name)
        {
            // les groupes de dés nommés :
            // ils sont case-sensistive, mais "mon jeu" == "mon jeu " == "  mon jeu"
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "param should not be null or empty");
            }
            return Task.FromResult(diceGroups.First(diceGroup => diceGroup.Name.Equals(name.Trim())));
        }

        public void Remove(DiceGroup toRemove)
        {
            if (toRemove.Name is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            else
            {
                diceGroups.Remove(toRemove);
            }
        }

        /// <summary>
        /// updates a (string, ReadOnlyCollection-of-Die) couple. only the name can be updated
        /// </summary>
        /// <param name="before">original couple</param>
        /// <param name="after">new couple</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<DiceGroup> Update(DiceGroup before, DiceGroup after)
        {
            // pas autorisé de changer les dés, juste le nom
            if (!before.Dice.SequenceEqual(after.Dice))
            {
                throw new ArgumentException("the group of dice cannot be updated, only the name", nameof(before));
            }
            if (string.IsNullOrWhiteSpace(before.Name) || string.IsNullOrWhiteSpace(after.Name))
            {
                throw new ArgumentNullException(nameof(before), "dice group name should not be null or empty");
            }
            Remove(before);
            Add(after);
            return Task.FromResult(after);

        }
    }
}
