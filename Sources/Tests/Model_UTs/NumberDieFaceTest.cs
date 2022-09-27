﻿using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs
{
    public class NumberDieFaceTest
    {

        [Fact]
        public void TestCreatNumFace()
        {
            //Arrange
            NumberDieFace face;
            int expected = 11;

            //Act
            face = new NumberDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel); 
        }

        [Fact]
        public void TestGetPracticalValueNumFace()
        {
            //Arrange
            NumberDieFace face;
            int expected = 11;

            //Act
            face = new NumberDieFace(expected);
            int actuel = (int)face.GetPracticalValue();

            //Assert
            Assert.Equal(expected, actuel);
        }

        [Fact]
        public void TestNumberFaceToString()
        {
            //Arrange
            NumberDieFace face;
            int expected = 11;

            //Act
            face = new NumberDieFace(expected);
            string actuel = face.ToString();

            //Assert
            Assert.Equal(expected.ToString(), actuel);

        }

    }
}