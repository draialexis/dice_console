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

            await dgm.AddCheckName(new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) }));
            DiceGroup group1 = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            async Task actionAsync() => await dgm.AddCheckName(group1);

            // Assert
            await Xunit.Assert.ThrowsAsync<ArgumentException>(actionAsync);

        }

        [Fact]
        public async Task TestGetOneByIdThrowsException()
        {
            // Arrange
            DiceGroupManager dgm = new();

            // Act

            async Task action() => dgm.GetOneByID(new("9657b6f0-9431-458e-a2bd-4bb0c7d4a6ed"));

            // Assert
           await Xunit.Assert.ThrowsAsync<NotImplementedException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task TestGetOneByNameIfInvalidThrowsException(string name)
        {
            // Arrange
            DiceGroupManager dgm = new();
            DiceGroup toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            
            await dgm.Add(toAdd);
            async Task action() => await dgm.GetOneByName(name);

            // Assert
           await Xunit.Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TestRemoveWorksIfExists()
        {
            // Arrange
            DiceGroupManager dgm = new();
            // KeyValuePair<string, IEnumerable<Die>> toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            DiceGroup diceGroup = new ("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            await dgm.Add(diceGroup);
            dgm.Remove(diceGroup);

            Xunit.Assert.DoesNotContain(diceGroup, await dgm.GetAll());
        }

        [Fact]
        public async void TestRemoveFailsSilentlyIfGivenNonExistent()
        {
            DiceGroupManager dgm = new();
            DiceGroup toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            await dgm.Add(toAdd);

            DiceGroup toAdd2 = new("Scrabble", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });

            // Act
            dgm.Remove(toAdd2);
           
            // Assert
            Xunit.Assert.DoesNotContain(toAdd2, await dgm.GetAll());
        }

        //To check sequence equal does not work properly
        [Fact]
        public async Task TestUpdateWorksIfValid()
        {
            // Arrange
            DiceGroupManager dgm = new();
            DiceGroup toAdd = new("Monopoly", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            await dgm.Add(toAdd);
            DiceGroup toAdd2 = new("Scrabble", new List<NumberDie> { new NumberDie(new NumberFace(5), new NumberFace(7)), new NumberDie(new NumberFace(5), new NumberFace(7)) });
            await dgm.Update(toAdd, toAdd2);

            Xunit.Assert.DoesNotContain(toAdd, await dgm.GetAll());
            Xunit.Assert.Contains(toAdd2, await dgm.GetAll());
        }

        /*        [Fact]
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
                }*/
        /* 


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
