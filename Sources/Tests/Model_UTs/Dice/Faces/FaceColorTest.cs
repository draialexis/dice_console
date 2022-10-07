using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model.Dice.Faces;
using System.Drawing;

namespace Tests.Model_UTs.Dice.Faces
{
    public class FaceColorTest
    {
        [Fact]
        public void ColorFaceValueTest()
        {
            Color color1 = Color.FromName("Chocolate");
            Color color2 = Color.FromArgb(144, 255, 78, 240);

            //Arrage 
            ColorFace face1 = new(color1);
            ColorFace face2 = new(color2);

            //Act
            Color expected1 = color1;
            Color actual1 = face1.Value;

            Color expected2 = color2;
            Color actual2 = face2.Value;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }
    }
}
