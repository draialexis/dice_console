using Model.Dice.Faces;
using Model.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Drawing;

namespace Tests.Model_UTs.Dice
{
    public class ColorDieTest
    {
        public static IEnumerable<object[]> Data_Uri()
        {
            yield return new object[]
            {
                new ColorFace(Color.FromName("Chocolate")),
                new ColorFace(Color.FromName("Aqua")),
                new ColorFace(Color.FromName("Beige")),
                new ColorFace(Color.FromName("Black")),
                new ColorFace(Color.FromName("BurlyWood")),
            };
        }


        [Theory]
        [MemberData(nameof(Data_Uri))]
        public void RndmFaceTest(ColorFace f1, ColorFace f2, ColorFace f3, ColorFace f4, ColorFace f5)
        {
            //Arrange
            List<ColorFace> listFaces = new() {
                f1,f2,f3,f4,f5
            };
            ColorDie die = new(
                listFaces[1],
                listFaces[2],
                listFaces[3],
                listFaces[4]
                );


            //Act
            ColorFace actual = (ColorFace)die.GetRandomFace();



            //Assert
            Assert.Contains(listFaces, face => face == actual);


        }
    }
}
