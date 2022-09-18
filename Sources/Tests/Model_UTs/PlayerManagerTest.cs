using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace Tests.Model_UTs
{
    public class PlayerManagerTest
    {
        [Fact]
        public void TestConstructorReturnsEmptyHashSet()
        {
            // Arrange
            PlayerManager playerManager;
            HashSet<Player> expected;
            HashSet<Player> actual;

            // Act
            playerManager = new();
            expected = new();
            actual = (HashSet<Player>)playerManager.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddIfPlayersThenDoAddAndReturnPlayers()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player alice = new("Alice");
            Player bob = new("Bob");
            HashSet<Player> expected = new() { alice, bob };

            // Act
            HashSet<Player> actual = new()
            {
                playerManager.Add(ref alice),
                playerManager.Add(ref bob)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddIfNullThenDontAddAndReturnNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player expected;
            Player actual;

            // Act
            expected = null;
            actual = playerManager.Add(ref expected);

            // Assert
            Assert.Equal(expected, actual);
            Assert.DoesNotContain(expected, playerManager.GetAll());
        }

        // TODO update if we do implement it
        [Fact]
        public void TestGetOneByIdThrowsNotImplemented()
        {
            // Arrange
            PlayerManager playerManager = new();

            // Act
            void action() => playerManager.GetOneById(1);

            // Assert
            Assert.Throws<NotImplementedException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestGetOneByNameIfInvalidThenReturnNull(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            playerManager.Add(ref player);

            // Act
            Player result = playerManager.GetOneByName(name);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestGetOneByNameIfValidButNotExistThenReturnNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            playerManager.Add(ref player);

            // Act
            Player result = playerManager.GetOneByName("Clyde");

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("Bob")]
        [InlineData("bob")]
        [InlineData("bob ")]
        [InlineData(" boB ")]
        public void TestGetOneByNameIfValidThenReturnPlayer(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player expected = new("Bob");
            playerManager.Add(ref expected);

            // Act
            Player actual = playerManager.GetOneByName(name);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRemoveWorksIfExists()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player p1 = new("Dylan");
            playerManager.Add(ref p1);

            // Act
            playerManager.Remove(ref p1);

            // Assert
            Assert.DoesNotContain(p1, playerManager.GetAll());
        }

        [Fact]
        public void TestRemoveFailsSilentlyIfGivenNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Dylan");
            playerManager.Add(ref player);
            Player notPlayer = null;
            HashSet<Player> expected = new() { player };

            // Act
            playerManager.Remove(ref notPlayer);
            HashSet<Player> actual = (HashSet<Player>)playerManager.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestRemoveFailsSilentlyIfGivenNonExistent()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Dylan");
            playerManager.Add(ref player);
            Player notPlayer = new("Eric");
            HashSet<Player> expected = new() { player };

            // Act
            playerManager.Remove(ref notPlayer);
            HashSet<Player> actual = (HashSet<Player>)playerManager.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestUpdateWorksIfValid()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Dylan");
            playerManager.Add(ref oldPlayer);
            Player newPlayer = new("Eric");

            // Act
            playerManager.Update(ref oldPlayer, ref newPlayer);

            // Assert
            Assert.DoesNotContain(oldPlayer, playerManager.GetAll());
            Assert.Contains(newPlayer, playerManager.GetAll());
        }
    }
}
