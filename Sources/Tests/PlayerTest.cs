using Model;
using System;
using Xunit;

namespace Tests
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
