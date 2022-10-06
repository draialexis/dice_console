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



        [Theory]
        [InlineData(Color.FromName("Chocolate"))]
        [InlineData(Color.FromArgb(144, 255, 78, 240))]
        public void ColorFaceValueTest(Color color)
        {


            //Arrage 
            ColorFace face = new ColorFace(color);


            //Act
            Color expected = color;
            Color actual = face.Value;



            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
