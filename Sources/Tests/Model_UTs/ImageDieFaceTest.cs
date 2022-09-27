using Model.Dice.Faces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs
{
    public class ImageDieFaceTest
    {
        [Fact]
        public void TestCreatImageFace()
        {
            //Arrange
            ImageDieFace face;
            int expected = 11;

            //Act
            face = new ImageDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel);
        }

        [Fact]
        public void TestGetPracticalValueImageFace()
        {
            //Arrange
            ImageDieFace face;
            int expected = 11;

            //Act
            face = new ImageDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel);
        }

        [Fact]
        public void TestImageFaceToString()
        {
            //Arrange
            ImageDieFace face;
            string expected = "hello/world/11.jpg";

            //Act
            face = new ImageDieFace(expected);
            string actuel = string.Format("Assets/images/{0}", face.GetPracticalValue());

            //Assert
            Assert.Equal(expected.ToString(), actuel);

        }
    }
}
