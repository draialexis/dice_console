using Model;
using System;
using Xunit;

namespace Tests.Model_UTs
{
    public class PlayerTest
    {
        [Fact]
        public void TestConstructorIfNameThenCorrectName()
        {
            // Arrange
            Player player;

            // Act
            player = new("Alice");

            // Assert
            Assert.Equal("Alice", player.Name);
        }

        [Fact]
        public void TestConstructorIfWhitespaceThenException()
        {
            // Arrange
            Player player;

            // Act
            void action() => player = new(" ");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestConstructorIfBlankThenException()
        {
            // Arrange
            Player player;

            // Act
            void action() => player = new("");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestConstructorIfNullThenException()
        {
            // Arrange
            Player player;
            string name = null;

            // Act
            void action() => player = new Player(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestToStringCorrectName()
        {
            // Arrange
            string expected = "Bob";
            Player player = new(expected);

            // Act
            string actual = player.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x, int y)
            {
                X = x; Y = y;
            }
        }

        [Fact]
        public void TestEqualsFalseIfNotPlayer()
        {
            // Arrange
            Point point;
            Player player;

            // Act
            point = new(1, 2);
            player = new("Clyde");

            // Assert
            Assert.False(point.Equals(player));
            Assert.False(player.Equals(point));
        }

        [Fact]
        public void TestGoesThruIfObjIsPlayer()
        {
            // Arrange
            Object p1;
            Player p2;

            // Act
            p1 = new Player("Marvin");
            p2 = new("Clyde");

            // Assert
            Assert.False(p1.Equals(p2));
            Assert.False(p2.Equals(p1));
        }


        [Fact]
        public void TestEqualsFalseIfNotSameName()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Panama");
            p2 = new("Clyde");

            // Assert
            Assert.False(p1.Equals(p2));
            Assert.False(p2.Equals(p1));
        }

        [Fact]
        public void TestEqualsTrueIfSameNameDifferentCase()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Devon");
            p2 = new("devoN");

            // Assert
            Assert.True(p1.Equals(p2));
            Assert.True(p2.Equals(p1));
        }

        [Fact]
        public void TestEqualsTrueIfExactlySameName()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Elyse");
            p2 = new("Elyse");

            // Assert
            Assert.True(p1.Equals(p2));
            Assert.True(p2.Equals(p1));
        }

        [Fact]
        public void TestSameHashFalseIfNotSameName()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Panama");
            p2 = new("Clyde");

            // Assert
            Assert.False(p1.GetHashCode().Equals(p2.GetHashCode()));
            Assert.False(p2.GetHashCode().Equals(p1.GetHashCode()));
        }

        [Fact]
        public void TestSameHashTrueIfSameNameDifferentCase()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Devon");
            p2 = new("devoN");

            // Assert
            Assert.True(p1.GetHashCode().Equals(p2.GetHashCode()));
            Assert.True(p2.GetHashCode().Equals(p1.GetHashCode()));
        }

        [Fact]
        public void TestSameHashTrueIfExactlySameName()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Elyse");
            p2 = new("Elyse");

            // Assert
            Assert.True(p1.GetHashCode().Equals(p2.GetHashCode()));
            Assert.True(p2.GetHashCode().Equals(p1.GetHashCode()));
        }

        [Fact]
        public void TestCopyConstructorExactCopy()
        {
            // Arrange
            Player p1;
            Player p2;

            // Act
            p1 = new("Elyse");
            p2 = new(p1);

            // Assert
            Assert.True(p1.Equals(p2));
        }

        [Fact]
        public void TestCopyConstructorIfNullThenException()
        {
            // Arrange
            Player p1;
            Player p2 = null;

            // Act
            void action() => p1 = new Player(p2);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
