using Model.Dice.Faces;
using System.Collections.Generic;

namespace Model.Dice
{
    public class DieManager : IManager<(string, IEnumerable<AbstractDie<AbstractDieFace>>)>
    {
        private readonly List<(string name, IEnumerable<AbstractDie<AbstractDieFace>> dice)> diceGroups = new();

        public (string, IEnumerable<AbstractDie<AbstractDieFace>>) Add((string, IEnumerable<AbstractDie<AbstractDieFace>>) toAdd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<(string, IEnumerable<AbstractDie<AbstractDieFace>>)> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public (string, IEnumerable<AbstractDie<AbstractDieFace>>) GetOneByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void Remove((string, IEnumerable<AbstractDie<AbstractDieFace>>) toRemove)
        {
            throw new System.NotImplementedException();
        }

        public (string, IEnumerable<AbstractDie<AbstractDieFace>>) Update((string, IEnumerable<AbstractDie<AbstractDieFace>>) before, (string, IEnumerable<AbstractDie<AbstractDieFace>>) after)
        {
            throw new System.NotImplementedException();
        }
    }
}
