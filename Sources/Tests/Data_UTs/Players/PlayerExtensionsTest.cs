using Data.EF.Players;
using Model.Players;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Data_UTs.Players
{
    public class PlayerExtensionsTest
    {
        [Fact]
        public void TestToModel()
        {
            // Arrange
            string name = "Alice";
            PlayerEntity entity = new() { Name = name };
            Player expected = new(name);

            // Act
            Player actual = entity.ToModel();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToEntity()
        {
            // Arrange
            string name = "Bob";
            Player model = new(name);
            PlayerEntity expected = new() { Name = name };

            // Act
            PlayerEntity actual = model.ToEntity();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToModels()
        {
            // Arrange
            string n1 = "Alice", n2 = "Bob", n3 = "Clyde";
            PlayerEntity[] entities = new PlayerEntity[] {
                new() {Name = n1 },
                new() {Name = n2 },
                new() {Name = n3 },
                };

            IEnumerable<Player> expected = new Player[] {
                new(n1),
                new(n2),
                new(n3)
                }.AsEnumerable();

            // Act
            IEnumerable<Player> actual = entities.ToModels();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToEntities()
        {
            // Arrange
            string n1 = "Alice", n2 = "Bob", n3 = "Clyde";
            Player[] models = new Player[] {
                new(n1),
                new(n2),
                new(n3)
            };

            IEnumerable<PlayerEntity> expected = new PlayerEntity[] {
                new() {Name = n1 },
                new() {Name = n2 },
                new() {Name = n3 },
                }.AsEnumerable();

            // Act
            IEnumerable<PlayerEntity> actual = models.ToEntities();

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
