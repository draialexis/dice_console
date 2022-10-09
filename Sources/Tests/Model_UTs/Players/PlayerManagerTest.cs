using Model.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs.Players
{
    public class PlayerManagerTest
    {
        [Fact]
        public async Task TestConstructorReturnsEmptyEnumerableAsync()
        {
            // Arrange
            PlayerManager playerManager = new();
            IEnumerable<Player> expected;
            IEnumerable<Player> actual;

            // Act
            expected = new Collection<Player>();
            actual = await playerManager.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestAddIfPlayersThenDoAddAndReturnPlayersAsync()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player alice = new("Alice");
            Player bob = new("Bob");

            // Act
            Collection<Player> expected = new() { alice, bob };
            Collection<Player> actual = new()
            {
                await playerManager.Add(alice),
                await playerManager.Add(bob)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestAddIfNullThrowsException()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player expected;

            // Act
            expected = null;
            async Task actionAsync() => await playerManager.Add(expected);// Add() returns the added element if succesful

            // Assert
            Assert.Null(expected);
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync);
            Assert.DoesNotContain(expected, await playerManager.GetAll());
        }

        [Fact]
        public async Task TestAddIfAlreadyExistsThrowsException()
        {
            // Arrange
            PlayerManager playerManager = new();

            // Act
            await playerManager.Add(new("Kevin"));
            async Task actionAsync() => await playerManager.Add(new("Kevin"));

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(actionAsync);
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
        public async Task TestGetOneByNameIfInvalidThrowsException(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            await playerManager.Add(player);

            // Act
            async Task actionAsync() => await playerManager.GetOneByName(name);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(actionAsync);
        }

        [Fact]
        public void TestGetOneByNameIfValidButNotExistThenReturnNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Bob");
            playerManager.Add(player);

            // Act
            Player result = playerManager.GetOneByName("Clyde")?.Result;

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
            Player actual = playerManager.GetOneByName(name)?.Result;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestRemoveWorksIfExists()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player p1 = new("Dylan");
            await playerManager.Add(p1);

            // Act
            playerManager.Remove(p1);

            // Assert
            Assert.DoesNotContain(p1, await playerManager.GetAll());
        }

        [Fact]
        public async Task TestRemoveThrowsExceptionIfGivenNull()
        {
            // Arrange
            PlayerManager playerManager = new();
            await playerManager.Add(new Player("Dylan"));

            // Act
            void action() => playerManager.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public async Task TestRemoveFailsSilentlyIfGivenNonExistent()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player player = new("Dylan");
            await playerManager.Add(player);
            Player notPlayer = new("Eric");

            // Act
            playerManager.Remove(notPlayer);

            // Assert
            Assert.DoesNotContain(notPlayer, await playerManager.GetAll());
        }

        [Fact]
        public async Task TestUpdateWorksIfValid()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Dylan");
            await playerManager.Add(oldPlayer);
            Player newPlayer = new("Eric");

            // Act
            await playerManager.Update(oldPlayer, newPlayer);

            // Assert
            Assert.DoesNotContain(oldPlayer, await playerManager.GetAll());
            Assert.Contains(newPlayer, await playerManager.GetAll());
            Assert.True((await playerManager.GetAll()).Count() == 1);
        }

        [Theory]
        [InlineData("Filibert", "filibert")]
        [InlineData("Filibert", " fiLibert")]
        [InlineData("Filibert", "FIlibert ")]
        [InlineData(" Filibert", " filiBErt ")]
        public async Task TestUpdateDiscreetlyUpdatesCaseAndIgnoresExtraSpaceIfOtherwiseSame(string n1, string n2)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new(n1);
            await playerManager.Add(oldPlayer);
            Player newPlayer = new(n2);

            // Act
            await playerManager.Update(oldPlayer, newPlayer);

            // Assert
            Assert.Contains(oldPlayer, await playerManager.GetAll());
            Assert.Contains(newPlayer, await playerManager.GetAll());
            Assert.Equal(oldPlayer, newPlayer);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task TestUpdateDoesNotGoWithValidBeforeAndInvalidAfter(string badName)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ni!");
            await playerManager.Add(oldPlayer);
            int size1 = (await playerManager.GetAll()).Count();

            // Act
            Assert.Contains(oldPlayer, await playerManager.GetAll());
            async Task actionAsync() => await playerManager.Update(oldPlayer, new Player(badName));// this is really testing the Player class...
            int size2 = (await playerManager.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(actionAsync); // thrown by Player constructor
            Assert.Contains(oldPlayer, await playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Fact]
        public async Task TestUpdateDoesNotGoWithValidBeforeAndNullAfter()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ni!");
            await playerManager.Add(oldPlayer);
            int size1 = (await playerManager.GetAll()).Count();

            // Act
            Assert.Contains(oldPlayer, await playerManager.GetAll());
            async Task actionAsync() => await playerManager.Update(oldPlayer, null);
            int size2 = (await playerManager.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync); // thrown by Update()
            Assert.Contains(oldPlayer, await playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Fact]
        public async Task TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange
            PlayerManager playerManager = new();
            Player newPlayer = new("Kevin");
            Player oldPlayer = new("Ursula");
            await playerManager.Add(oldPlayer);
            int size1 = (await playerManager.GetAll()).Count();

            // Act
            async Task actionAsync() => await playerManager.Update(null, newPlayer);
            int size2 = (await playerManager.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(actionAsync); // thrown by Update()
            Assert.Contains(oldPlayer, await playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task TestUpdateDoesNotGoWithValidAfterAndInvalidBefore(string name)
        {
            // Arrange
            PlayerManager playerManager = new();
            Player oldPlayer = new("Ursula");
            await playerManager.Add(oldPlayer);
            int size1 = (await playerManager.GetAll()).Count();

            // Act
            async Task actionAsync() => await playerManager.Update(new Player(name), new Player("Vicky"));
            int size2 = (await playerManager.GetAll()).Count();

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(actionAsync); // thrown by Player constructor
            Assert.Contains(oldPlayer, await playerManager.GetAll()); // still there
            Assert.True(size1 == size2);
        }
    }
}
