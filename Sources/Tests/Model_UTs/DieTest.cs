using Model;
using Xunit;

namespace Tests.Model_UTs
{
    public class DieTest
    {
        [Fact]
        public void TestConstructor()
        {
            AbstractDie die = new AbstractDie("Ben");
            Assert.Equal("Ben", die.Name);
        }
    }
}
