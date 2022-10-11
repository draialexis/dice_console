using Model.Dice.Faces;
using Model.Dice;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Model_UTs.Dice
{
    public class ImageDieTest
    {


        public static IEnumerable<object[]> Data_Uri()
        {
            yield return new object[]
            {
                new ImageFace(new Uri("https://nothing1/")),
                new ImageFace(new Uri("https://nothing2/")),
                new ImageFace(new Uri("https://nothing3/")),
                new ImageFace(new Uri("https://nothing4/")),
                new ImageFace(new Uri("https://nothing5/")),
            };
        }


        [Theory]
        [MemberData(nameof(Data_Uri))]
        public void RndmFaceTest(ImageFace f1, ImageFace f2, ImageFace f3, ImageFace f4, ImageFace f5)
        {
            //Arrange
            List<ImageFace> listFaces = new() {
                f1,f2,f3,f4,f5
            };
            ImageDie die = new(
                listFaces[1],
                listFaces[2],
                listFaces[3],
                listFaces[4]
                );


            //Act
            ImageFace actual = (ImageFace)die.GetRandomFace();



            //Assert
            Assert.Contains(listFaces, face => face == actual);


        }
    }
}
