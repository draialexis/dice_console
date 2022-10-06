using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Tests.Model_UTs.Players
{
    public class PlayerManagerTest
    {
        [Fact]
        public void TestConstructorReturnsEmptyEnumerable()
        {
            // Arrange
            PlayerManager playerManager = new();
            IEnumerable<Player> expected;
            IEnumerable<Player> actual;

            // Act
            expected = new Collection<Player>();
            actual = playerManager.GetAll();

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

            // Act
            Collection<Player> expected = new() { alice, bob };
            Collection<Player> actual = new()
            {
                playerManager.Add(alice),
                playerManager.Add(bob)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddIfNullThrowsException()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player expected;

            // Act
            expected = null;
            void action() => playerManager.Add(expected);// Add() returns the added element if succesful

            // Assert
            Assert.Null(expected);
            Assert.Throws<ArgumentNullException>(action);
            Assert.DoesNotContain(expected, playerManager.GetAll());
        }

        [Fact]
        public void TestAddIfAlreadyExistsThrowsException()
        {
            // Arrange
            PlayerManager playerManager = new();

            // Act
            playerManager.Add(new("Kevin"));
            void action() => playerManager.Add(new("Kevin"));

            // Assert
            Assert.Throws<ArgumentException>(action);
        }


        [Fact]
        public void TestGetOneByIdThrowsException()
        {
            // Arrange
            PlayerManager playerManager = new();

            // Act
           
            void action() => playerManager.GetOneByID(new("1a276327-75fc-45b9-8854-e7c4101088f8"));

            // Assert
            Assert.Throws<NotImplementedException>(action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestGetOneByNameIfInvalidThrowsException(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            playerManager.Add(player);

            // Act
            void action() => playerManager.GetOneByName(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetOneByNameIfValidButNotExistThenReturnNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            playerManager.Add(player);

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
            playerManager.Add(expected);

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
            playerManager.Add(p1);

            // Act
            playerManager.Remove(p1);

            // Assert
            Assert.DoesNotContain(p1, playerManager.GetAll());
        }

        [Fact]
        public void TestRemoveThrowsExceptionIfGivenNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            playerManager.Add(new Player("Dylan"));

            // Act
            void action() => playerManager.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestRemoveFailsSilentlyIfGivenNonExistent()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Dylan");
            playerManager.Add(player);
            Player notPlayer = new("Eric");
            IEnumerable<Player> expected = new Collection<Player> { player };

            // Act
            playerManager.Remove(notPlayer);
            IEnumerable<Player> actual = playerManager.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestUpdateWorksIfValid()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Dylan");
            playerManager.Add(oldPlayer);
            Player newPlayer = new("Eric");

            // Act
            playerManager.Update(oldPlayer, newPlayer);

            // Assert
            Assert.DoesNotContain(oldPlayer, playerManager.GetAll());
            Assert.Contains(newPlayer, playerManager.GetAll());
            Assert.True(playerManager.GetAll().Count() == 1);
        }

        [Theory]
        [InlineData("Filibert", "filibert")]
        [InlineData("Filibert", " fiLibert")]
        [InlineData("Filibert", "FIlibert ")]
        [InlineData(" Filibert", " filiBErt ")]
        public void TestUpdateDiscreetlyUpdatesCaseAndIgnoresExtraSpaceIfOtherwiseSame(string n1, string n2)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new(n1);
            playerManager.Add(oldPlayer);
            Player newPlayer = new(n2);

            // Act
            playerManager.Update(oldPlayer, newPlayer);

            // Assert
            Assert.Contains(oldPlayer, playerManager.GetAll());
            Assert.Contains(newPlayer, playerManager.GetAll());
            Assert.Equal(oldPlayer, newPlayer);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateDoesNotGoWithValidBeforeAndInvalidAfter(string badName)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ni!");
            playerManager.Add(oldPlayer);
            int size1 = playerManager.GetAll().Count();

            // Act
            Assert.Contains(oldPlayer, playerManager.GetAll());
            void action() => playerManager.Update(oldPlayer, new Player(badName));// this is really testing the Player class...
            int size2 = playerManager.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by Player constructor
            Assert.Contains(oldPlayer, playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidBeforeAndNullAfter()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ni!");
            playerManager.Add(oldPlayer);
            int size1 = playerManager.GetAll().Count();

            // Act
            Assert.Contains(oldPlayer, playerManager.GetAll());
            void action() => playerManager.Update(oldPlayer, null);
            int size2 = playerManager.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by Update()
            Assert.Contains(oldPlayer, playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player newPlayer = new("Kevin");
            Player oldPlayer = new("Ursula");
            playerManager.Add(oldPlayer);
            int size1 = playerManager.GetAll().Count();

            // Act
            void action() => playerManager.Update(null, newPlayer);
            int size2 = playerManager.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentNullException>(action); // thrown by Update()
            Assert.Contains(oldPlayer, playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateDoesNotGoWithValidAfterAndInvalidBefore(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ursula");
            playerManager.Add(oldPlayer);
            int size1 = playerManager.GetAll().Count();

            // Act
            void action() => playerManager.Update(new Player(name), new Player("Vicky"));
            int size2 = playerManager.GetAll().Count();

            // Assert
            Assert.Throws<ArgumentException>(action); // thrown by Player constructor
            Assert.Contains(oldPlayer, playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }
    }
}
