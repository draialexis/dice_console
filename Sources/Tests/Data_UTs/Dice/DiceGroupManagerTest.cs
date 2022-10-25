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
        private static readonly Task<MasterOfCeremonies> stubGameRunner = new Stub().LoadApp();
        [Fact]
        public void TestConstructorReturnsEmptyEnumerable()
        {

            // Arrange
            DiceGroupManager diceGroupManager = new();

            List<DiceGroup> expected;
            Task<ReadOnlyCollection<DiceGroup>> actual;

            // Act
            expected = new List<DiceGroup>();
            actual =  diceGroupManager.GetAll();

            // Assert
            Xunit.Assert.Equal(expected, actual.Result);

        }

 [Fact]

        public async Task TestAddWhenDiceGroupThenDoAddAndReturnDiceGroup()
        {
            // Arrange
            DiceGroupManager dgm = new();
            DiceGroup group1 = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            DiceGroup group2 = new("Scrabble", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            //...storing the results of DiceGroupManager.Add() in variables 
            Collection<DiceGroup> expected = new() {group1,group2 };
            Collection<DiceGroup> actual = new()
            {
                await dgm.Add(group1),
                await dgm.Add(group2)
            };
            // Assert
            Xunit.Assert.Equal(expected, actual);
            Xunit.Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task TestAddIfNullThrowsException()
        {
            DiceGroupManager dgm = new();
            DiceGroup expected;

            expected = null;
            async Task actionAsync() => await dgm.Add(expected);// Add() returns the added element if succesful

            // Assert
            Xunit.Assert.Null(expected);
            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
            Xunit.Assert.DoesNotContain(expected, await dgm.GetAll());
        }
        [Fact]
        public async Task TestAddIfAlreadyExistsThrowsException()
        {
            DiceGroupManager dgm = new();

            await dgm.Add(new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) }));
            DiceGroup group1 = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            async Task actionAsync() => await dgm.Add(group1);

            // Assert
            await Xunit.Assert.ThrowsAsync<Exception>(actionAsync);

        }

        /* 

  [Fact]
  public void TestAddIfAlreadyExistsThrowsException()
  {
      DiceGroupManager dgm = new();

      // Act
      KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
      DiceGroup diceGroup = new DiceGroup("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

      dgm.Add(diceGroup);

      void action() => dgm.Add(diceGroup);

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
      DiceGroup diceGroup = new DiceGroup("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

      dgm.Add(diceGroup);
      void action() => dgm.GetOneByName(name);

      // Assert
      Xunit.Assert.Throws<ArgumentNullException>(action);
  }

  [Fact]
  public void TestRemoveWorksIfExists()
  {
      // Arrange
      DiceGroupManager dgm = new();
      // KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
      DiceGroup diceGroup = new DiceGroup("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

      dgm.Add(diceGroup);
      dgm.Remove(diceGroup);

      Xunit.Assert.DoesNotContain(diceGroup, (IEnumerable<DiceGroup>)dgm.GetAll());
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

*/












    }
}
