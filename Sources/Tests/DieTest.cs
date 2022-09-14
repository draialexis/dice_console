using Model;
using System;
using Xunit;

namespace Tests
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
