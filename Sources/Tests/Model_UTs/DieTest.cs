using Model;
using Xunit;

namespace Tests.Model_UTs
{
    public class DieTest
    {
        [Fact]
        public void TestConstructor()
        {
            Die die = new Die("Jerry");
            Assert.Equal("Jerry", die.Name);
        }
    }
}
