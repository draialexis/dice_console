using Data.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Dice;
using Model.Dice.Faces;
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
        public void TestAddIfDieGroupThenDoAddAndDieGroup()
        {
            List<NumberFace> faces = new ();
            faces.Add(new NumberFace(2));
            faces.Add(new NumberFace(3));
            faces.Add(new NumberFace(7));

            NumberFace[] facesArr = faces.ToArray();
            faces.Clear();


            /*            facesArr[0] = new NumberFace(2);
                        facesArr[1] = new NumberFace(3);
                        facesArr[2] = new NumberFace(7);*/
            faces.Add(new NumberFace(3));
            faces.Add(new NumberFace(5));
            faces.Add(new NumberFace(6));

            NumberFace[] facesArr1 = faces.ToArray();

            /*facesArr1[0] = new NumberFace(3);
            facesArr1[1] = new NumberFace(5);
            facesArr1[2] = new NumberFace(6);
*/
            Collection<Die> collectDie = new();



            Dictionary<string, IEnumerable<Die>> diceGroupsActuel = new();

            NumberDie die = new NumberDie(facesArr[0], facesArr[1..]);
            NumberDie die2 = new NumberDie(facesArr1[0], facesArr1[1..]);

            collectDie.Add(die);
            collectDie.Add(die2);


            Collection<Die> ExpectedDie = new();
            ExpectedDie.Add(new NumberDie(new NumberFace(2), new NumberFace(3), new NumberFace(7)));
            ExpectedDie.Add(new NumberDie(new NumberFace(3), new NumberFace(5), new NumberFace(6)));


            Dictionary<string, IEnumerable<Die>> diceGroupExpected = new();//to ask direct initialization why cannot
            diceGroupExpected.Add("Monopoly", ExpectedDie);
            diceGroupsActuel.Add("Monopoly", collectDie);

            CollectionAssert.AreEqual(collectDie, ExpectedDie);

        }


    }
}
