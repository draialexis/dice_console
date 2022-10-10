using Model.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model.Dice.Faces;
using System.Diagnostics;

namespace Tests.Model_UTs.Dice
{
    public class NumberDieTest
    {
        [Fact]
        public void RndmFaceTest()
        {
            //Arrange
            List<NumberFace> listFaces = new() {
                new NumberFace(1),
                new NumberFace(2),
                new NumberFace(3),
                new NumberFace(4),
                new NumberFace(5),
            };
            NumberDie die = new(
                listFaces[1],
                listFaces[2],
                listFaces[3],
                listFaces[4]
                );


            //Act
            NumberFace actual = (NumberFace)die.GetRandomFace();



            //Assert
            Assert.Contains(listFaces, face =>face==actual);


        }
    }
}
