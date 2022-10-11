using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Model_UTs.Dice.Faces
{
    public class ImageFaceTest
    {
        public static IEnumerable<object[]> Data_Colors()
        {
            yield return new object[]
            {
                new Uri("http://www.contoso.com/"),
                new Uri("https://www.pedagojeux.fr/wp-content/uploads/2019/11/1280x720_LoL.jpg"),
            };
            yield return new object[]
            {
                new Uri("https://www.lacremedugaming.fr/wp-content/uploads/creme-gaming/2022/02/media-13411.jpg"),
                new Uri("https://i1.moyens.net/io/images/2022/01/1642321015_Mises-a-jour-de-VALORANT-en-melee-a-venir-dans.jpg"),
            };
        }


        [Theory]
        [MemberData(nameof(Data_Colors))]
        public void ImageFaceValueTest(Uri uriA, Uri uriB)
        {


            //Arrage 
            ImageFace face1 = new(uriA);
            ImageFace face2 = new(uriB);

            //Act
            Uri expected1 = uriA;
            Uri actual1 = face1.Value;

            Uri expected2 = uriB;
            Uri actual2 = face2.Value;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }


        [Theory]
        [MemberData(nameof(Data_Colors))]
        public void ImageFaceValueToStringTest(Uri uriA, Uri uriB)
        {


            //Arrage 
            ImageFace face1 = new(uriA);
            ImageFace face2 = new(uriB);

            //Act
            string expected1 = uriA.ToString();
            string actual1 = face1.StringValue;

            string expected2 = uriB.ToString();
            string actual2 = face2.StringValue;

            //Assert
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }
    }
}
