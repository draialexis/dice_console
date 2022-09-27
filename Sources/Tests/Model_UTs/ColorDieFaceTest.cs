using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs
{
    public class ColorDieFaceTest
    {
        [Fact]
        public void TestCreatColorFace()
        {
            //Arrange
            ColorDieFace face;
            int expected = 11;

            //Act
            face = new ColorDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel);
        }

        [Fact]
        public void TestGetPracticalValueColorFace()
        {
            //Arrange
            ColorDieFace face;
            int expected = 11;

            //Act
            face = new ColorDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel);
        }

        [Fact]
        public void TestColorFaceToString()
        {
            //Arrange
            ColorDieFace face;
            int expected = 11;

            //Act
            face = new ColorDieFace(expected);
            string actuel = face.ToString();

            //Assert
            Assert.Equal(expected.ToString(), actuel);

        }
    }
}
