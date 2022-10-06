using Data.EF.Players;
using System;
using Tests.Model_UTs;
using Xunit;

namespace Tests.Data_UTs.Players
{
    public class PlayerEntityTest
    {
        [Fact]
        public void TestGetSetName()
        {
            // Arrange
            PlayerEntity player = new();
            string expected = "Alice";

            // Act
            player.Name = expected;
            string actual = player.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetSetID()
        {
            // Arrange
            PlayerEntity player = new();
            Guid expected = Guid.NewGuid();

            // Act
            player.ID = expected;
            Guid actual = player.ID;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString()
        {
            // Arrange
            PlayerEntity player = new();
            string IDString = "c8f60957-dd36-4e47-a7ce-1281f4f8bea4";
            string nameString = "Bob";
            player.ID = new Guid(IDString);
            player.Name = nameString;

            // Act
            string expected = $"{IDString.ToUpper()} -- {nameString}";

            // Assert
            Assert.Equal(expected, player.ToString());
        }

        [Fact]
        public void TestEqualsWhenNotPlayerEntityThenFalse()
        {
            // Arrange
            Point point;
            PlayerEntity entity;

            // Act
            point = new(1, 2);
            entity = new() { Name = "Clyde" };

            // Assert
            Assert.False(point.Equals(entity));
            Assert.False(entity.Equals(point));
        }

        [Fact]
        public void TestEqualsWhenNullThenFalse()
        {
            // Arrange
            PlayerEntity entity;

            // Act
            entity = new() { Name = "Clyde" };

            // Assert
            Assert.False(entity.Equals(null));
        }

        [Fact]
        public void TestGoesThruToSecondMethodIfObjIsTypePlayerEntity()
        {
            // Arrange
            object p1;
            PlayerEntity p2;

            // Act
            p1 = new PlayerEntity() { Name = "Marvin" };
            p2 = new() { Name = "Clyde" };

            // Assert
            Assert.False(p1.Equals(p2));
            Assert.False(p2.Equals(p1));
        }

        [Fact]
        public void TestEqualsFalseIfNotSameNameOrID()
        {
            // Arrange
            PlayerEntity p1;
            PlayerEntity p2;
            PlayerEntity p3;

            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            string name1 = "Panama";
            string name2 = "Clyde";

            // Act
            p1 = new() { ID = id1, Name = name1 };
            p2 = new() { ID = id1, Name = name2 };
            p3 = new() { ID = id2, Name = name2 };

            // Assert
            Assert.False(p1.Equals(p2));
            Assert.False(p1.Equals(p3));
            Assert.False(p2.Equals(p1));
            Assert.False(p2.Equals(p3));
            Assert.False(p3.Equals(p1));
            Assert.False(p3.Equals(p2));
        }

        [Fact]
        public void TestEqualsTrueIfSameIDAndName()
        {
            // Arrange
            PlayerEntity p1;
            PlayerEntity p2;
            Guid id = Guid.NewGuid();
            string name = "Marley";

            // Act
            p1 = new() { ID = id, Name = name };
            p2 = new() { ID = id, Name = name };

            // Assert
            Assert.True(p1.Equals(p2));
            Assert.True(p2.Equals(p1));
        }

        [Fact]
        public void TestSameHashFalseIfNotSameNameOrID()
        {
            // Arrange
            PlayerEntity p1;
            PlayerEntity p2;
            PlayerEntity p3;

            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            string name1 = "Panama";
            string name2 = "Clyde";

            // Act
            p1 = new() { ID = id1, Name = name1 };
            p2 = new() { ID = id1, Name = name2 };
            p3 = new() { ID = id2, Name = name2 };

            // Assert
            Assert.False(p1.GetHashCode().Equals(p2.GetHashCode()));
            Assert.False(p1.GetHashCode().Equals(p3.GetHashCode()));
            Assert.False(p2.GetHashCode().Equals(p1.GetHashCode()));
            Assert.False(p2.GetHashCode().Equals(p3.GetHashCode()));
            Assert.False(p3.GetHashCode().Equals(p1.GetHashCode()));
            Assert.False(p3.GetHashCode().Equals(p2.GetHashCode()));
        }

        [Fact]
        public void TestSameHashTrueIfSame()
        {
            // Arrange
            PlayerEntity p1;
            PlayerEntity p2;
            Guid id = Guid.NewGuid();
            string name = "Marley";

            // Act
            p1 = new() { ID = id, Name = name };
            p2 = new() { ID = id, Name = name };

            // Assert
            Assert.True(p1.GetHashCode().Equals(p2.GetHashCode()));
            Assert.True(p2.GetHashCode().Equals(p1.GetHashCode()));
        }
    }
}
