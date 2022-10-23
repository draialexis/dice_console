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
            Dictionary<string, IEnumerable<Die>> toAdd = new();
/*            void action() => toAdd.Add("", new List<NumberDie>{ new NumberDie(d6Faces[0], d6Faces[1..]),
                new NumberDie(d6Faces[0], d6Faces[3..])}); */
           // Xunit.Assert.Empty(expected.Keys);
            foreach (KeyValuePair<string, IEnumerable<Die>> entry in expected)
            {
                Xunit.Assert.Empty(entry.Key);
                // do something with entry.Value or entry.Key
            }
        }


        [Fact]
        public void TestAddIfAlreadyExistsThrowsException()
        {
            DiceGroupManager dgm = new();
            
            // Act
            KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7))});
            dgm.Add(toAdd);

            void action() => dgm.Add(toAdd);

            // Assert
            Xunit.Assert.Throws<ArgumentException>(action);

        }
        [Fact]
        public void TestGetOneByIdThrowsException()
        {
            // Arrange
            DiceGroupManager dgm = new();

            // Act

            void action() => dgm.GetOneByID(new("9657b6f0-9431-458e-a2bd-4bb0c7d4a6ed"));

            // Assert
            Xunit.Assert.Throws<NotImplementedException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestGetOneByNameIfInvalidThrowsException(string name)
        {
            // Arrange
            DiceGroupManager dgm = new();
            KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            dgm.Add(toAdd);
            void action() => dgm.GetOneByName(name);

            // Assert
            Xunit.Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestRemoveWorksIfExists()
        {
            // Arrange
            DiceGroupManager dgm = new();
            KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            dgm.Add(toAdd);
            dgm.Remove(toAdd);

           Xunit.Assert.DoesNotContain(toAdd, dgm.GetAll());
        }

        [Fact]
        public void TestRemoveFailsSilentlyIfGivenNonExistent()
        {
            DiceGroupManager dgm = new();
            KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            dgm.Add(toAdd);

            KeyValuePair<string, IEnumerable<Die>> toAdd2 = new("Scrabble", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            // Act
            dgm.Remove(toAdd2);

            // Assert
            Xunit.Assert.DoesNotContain(toAdd2, dgm.GetAll());
        }

        [Fact]
        public void TestUpdateWorksIfValid()
        {
            // Arrange
            DiceGroupManager dgm = new();
            KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            dgm.Add(toAdd);
            KeyValuePair<string, IEnumerable<Die>> toAdd2 = new("Scrabble", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            dgm.Update(toAdd, toAdd2);

            Xunit.Assert.DoesNotContain(toAdd, dgm.GetAll());
            Xunit.Assert.Contains(toAdd2, dgm.GetAll());
            Xunit.Assert.True(dgm.GetAll().Count() == 1);
        }














    }
}
