using Model.Dice;
using Model.Dice.Faces;
using Xunit;

namespace Tests.Model_UTs.Dice
{
    public class NumberDieTest
    {

        [Fact]
        public void TestGetRandomFace()
        {
            // Arrange
            int val1 = 1, val2 = 2;
            NumberDie die = new(new NumberFace(val1), new NumberFace(val2));

            // Act

            Face<int> thing = die.GetRandomFace();

            // Assert

            Assert.IsType<NumberFace>(thing);
            Assert.True(thing.Value == val1 || thing.Value == val2);
        }
    }
}
