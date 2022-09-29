﻿using Model.Games;
using Model.Dice;
using Model.Dice.Faces;
using Model.Players;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model_UTs
{
    public class GameRunnerTest
    {
        private readonly GameRunner stubGameRunner;
        public GameRunnerTest()
        {
            stubGameRunner = new Stub().LoadApp();
        }

        [Fact]
        public void TestConstructorWhenNoGamesThenNewIEnumerable()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act
            expected = new List<Game>().AsEnumerable();
            actual = gameRunner.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestConstructorWhenGamesThenGamesIEnumerable()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager(), stubGameRunner.GetAll().ToList());
            IEnumerable<Game> expected;
            IEnumerable<Game> actual;

            // Act
            expected = stubGameRunner.GetAll();
            actual = gameRunner.GetAll();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenGamesThenDoAddAndReturnGames()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            Game game1 = stubGameRunner.GetAll().First();
            Game game2 = stubGameRunner.GetAll().Last();

            // Act
            IEnumerable<Game> expected = new List<Game>() { game1, game2 }.AsEnumerable();
            IEnumerable<Game> actual = new List<Game>()
            {
                gameRunner.Add(game1),
                gameRunner.Add(game2)
            };

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddWhenNullThenThrowsException()
        {
            // Arrange


            // Act
            void action() => stubGameRunner.Add(null);// Add() returns the added element if succesful

            // Assert
            Assert.Throws<ArgumentNullException>(action);
            Assert.DoesNotContain(null, stubGameRunner.GetAll());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void TestGetOneByNameWhenInvalidThenThrowsException(string name)
        {
            // Arrange


            // Act
            void action() => stubGameRunner.GetOneByName(name);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void TestGetOneByNameWhenValidButNotExistThenReturnNull()
        {
            // Arrange


            // Act
            Game result = stubGameRunner.GetOneByName("thereisbasicallynowaythatthisgamenamealreadyexists");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestGetOneByNameWhenValidThenReturnGame()
        {
            // Arrange
            GameRunner gameRunner = new(new PlayerManager(), new DieManager());
            Game game = stubGameRunner.GetAll().First();

            // Act
            Game actual = gameRunner.Add(game);
            Game expected = game;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestWhenRemoveExistsThenSucceeds()
        {
            // Arrange
            Game game = new("blargh", new PlayerManager(), stubGameRunner.GetAll().First().Dice);
            stubGameRunner.Add(game);

            // Act
            stubGameRunner.Remove(game);

            // Assert
            Assert.DoesNotContain(game, stubGameRunner.GetAll());
        }

        [Fact]
        public void TestRemoveWhenGivenNullThenThrowsException()
        {
            // Arrange


            // Act
            void action() => stubGameRunner.Remove(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TestRemoveWhenGiveenNonExistentThenFailsSilently()
        {
            // Arrange
            Game notGame = new("blargh", new PlayerManager(), stubGameRunner.GetAll().First().Dice);
            IEnumerable<Game> expected = stubGameRunner.GetAll();

            // Act
            stubGameRunner.Remove(notGame);
            IEnumerable<Game> actual = stubGameRunner.GetAll();

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void TestUpdateWorksIfValid()
        {
            // Arrange


            // Act


            // Assert

        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateDoesNotGoWithValidBeforeAndInvalidAfter(string badName)
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidBeforeAndNullAfter()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public void TestUpdateDoesNotGoWithValidAfterAndNullBefore()
        {
            // Arrange


            // Act


            // Assert

        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestUpdateDoesNotGoWithValidAfterAndInvalidBefore(string name)
        {
            // Arrange
            

            // Act
            

            // Assert
            
        }
    }
}
