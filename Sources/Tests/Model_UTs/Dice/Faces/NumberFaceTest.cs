using Model.Dice.Faces;
using Xunit;

namespace Tests.Model_UTs.Dice.Faces
{
    public class NumberFaceTest
    {
        [Fact]
        public void NumberFaceValueTest()
        {


            //Arrage 
            NumberFace face1 = new NumberFace(3);
            NumberFace face2 = new NumberFace(5);

            //Act
            int expected1 = 3;
            int actual1 = face1.Value;

            int expected2 = 5;
            int actual2 = face2.Value;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }


        [Fact]
        public void NumberFaceValueToStringTest()
        {


            //Arrage 
            NumberFace face1 = new(3);
            NumberFace face2 = new(5);

            //Act
            string expected1 = 3.ToString();
            string actual1 = face1.StringValue;

            string expected2 = 5.ToString();
            string actual2 = face2.StringValue;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }
    }
}
