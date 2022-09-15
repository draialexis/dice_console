using Model;
using Xunit;

namespace Tests.Model_UTs
{
    public class PlayerTest
    {
        [Fact]
        public void TestConstructor()
        {
            Player player = new Player("Alice");
            Assert.Equal("Alice", player.Name);
        }
    }
}
