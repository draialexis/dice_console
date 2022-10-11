using Model.Dice.Faces;
using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Tests.Model_UTs.Dice.Faces
{
    public class ColorFaceTest
    {
        public static IEnumerable<object[]> Data_Colors()
        {
            yield return new object[]
            {
                Color.FromName("Chocolate"),
                Color.FromArgb(144, 255, 78, 240),
            };
            yield return new object[]
            {
                Color.FromName("Chocolate"),
                Color.FromArgb(144, 255, 78, 240),
            };
        }


        [Theory]
        [MemberData(nameof(Data_Colors))]
        public void ColorFaceValueTest(Color clrA, Color clrB)
        {


            //Arrage 
            ColorFace face1 = new(clrA);
            ColorFace face2 = new(clrB);

            //Act
            Color expected1 = clrA;
            Color actual1 = face1.Value;

            Color expected2 = clrB;
            Color actual2 = face2.Value;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }


        [Theory]
        [MemberData(nameof(Data_Colors))]
        public void ColorFaceValueToStringTest(Color clrA, Color clrB)
        {


            //Arrage 
            ColorFace face1 = new(clrA);
            ColorFace face2 = new(clrB);

            //Act
            string expected1 = clrA.ToString();
            string actual1 = face1.StringValue;

            string expected2 = clrB.ToString();
            string actual2 = face2.StringValue;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }
    }
}
