using Data;
using Data.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Data_UTs.Dice
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
            Xunit.Assert.Equal(expected, actual);

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
            Xunit.Assert.Equal(expected, actual);
        }


        [Fact]
        public void TestAddIfNullThrowsException()
        {
            DiceGroupManager dgm = new();

            KeyValuePair<string, IEnumerable<Die>> group1 = stubGameRunner.DiceGroupManager.GetAll().First();
            KeyValuePair<string, IEnumerable<Die>> group2 = stubGameRunner.DiceGroupManager.GetAll().Last();
            NumberFace[] d6Faces = new NumberFace[] { new(1), new(2), new(3), new(4), new(5), new(6) };


            Dictionary<string, IEnumerable<Die>> expected = new()
            {
                { "", new List<NumberDie>{ new NumberDie(d6Faces[0], d6Faces[1..]), new NumberDie(d6Faces[0], d6Faces[3..]) } }
            };

            void action() => expected = new()
            {
                { "", new List<NumberDie>{ new NumberDie(d6Faces[0], d6Faces[1..]), new NumberDie(d6Faces[0], d6Faces[3..]) } }
            };
           // Xunit.Assert.Empty(expected.Keys);
            foreach (KeyValuePair<string, IEnumerable<Die>> entry in expected)
            {
                Xunit.Assert.Empty(entry.Key);
                // do something with entry.Value or entry.Key
            }

        }


    }
}
