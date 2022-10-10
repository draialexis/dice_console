using Data;
using Model.Dice;
using Model.Games;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs.Dice
{
    public class DiceGroupManagerTest
    {

        private readonly MasterOfCeremonies stubGameRunner = new Stub().LoadApp();

        [Fact]
        public void TestConstructorReturnsEmptyEnumerable()
        {
            // Arrange
            DiceGroupManager diceGroupManager = new();
            Dictionary<string, IEnumerable<Die>> expected;
            Dictionary<string, IEnumerable<Die>> actual;

            // Act
            expected = new Dictionary<string, IEnumerable<Die>>();
            actual = (Dictionary<string, IEnumerable<Die>>)diceGroupManager.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenDiceGroupThenDoAddAndReturnDiceGroup()
        {
            // Arrange
            DiceGroupManager dgm = new();
            KeyValuePair<string, IEnumerable<Die>> group1 = stubGameRunner.DiceGroupManager.GetAll().First();
            KeyValuePair<string, IEnumerable<Die>> group2 = stubGameRunner.DiceGroupManager.GetAll().Last();

            // Act

            //...adding keys and values to some dictionary for future comparison
            Dictionary<string, IEnumerable<Die>> expected = new()
            {
                { group1.Key, group1.Value },
                { group2.Key, group2.Value }
            };

            //...storing the results of DiceGroupManager.Add() in variables 
            KeyValuePair<string, IEnumerable<Die>> resultFromAdd1 = dgm.Add(group1);
            KeyValuePair<string, IEnumerable<Die>> resultFromAdd2 = dgm.Add(group2);

            //...using those variables to fill a second dictionary for comparison
            Dictionary<string, IEnumerable<Die>> actual = new()
            {
                { resultFromAdd1.Key, resultFromAdd1.Value },
                { resultFromAdd2.Key, resultFromAdd2.Value }
            };

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
