﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Players
{
    public class PlayerManager : IManager<Player>
    {
        /// <summary>
        /// a collection of the players that this manager is in charge of
        /// </summary>
        private readonly List<Player> players = new();

        /// <summary>
        /// add a new player
        /// </summary>
        /// <param name="toAdd">player to be added</param>
        /// <returns>added player</returns>
        public Task<Player> Add(Player toAdd)
        {
            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd), "param should not be null");
            }
            if (players.Contains(toAdd))
            {
                throw new ArgumentException("this username is already taken", nameof(toAdd));
            }
            players.Add(toAdd);
            return Task.FromResult(toAdd);
        }

        /// <summary>
        /// finds the player with that name and returns it
        /// </summary>
        /// <param name="name">a player's unique name</param>
        /// <returns>player with said name, <em>or null</em> if no such player was found</returns>
        public Task<Player> GetOneByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("param should not be null or blank", nameof(name));
            }

            Player result = players.FirstOrDefault(p => p.Name.ToUpper().Equals(name.ToUpper().Trim()));
            if (result == null)
            {
                return Task.FromResult<Player>(null);
            }
            return Task.FromResult(result);
        }

        /// </summary>
        /// get a READ ONLY enumerable of all players belonging to this manager
        /// so that the only way to modify the collection of players is to use this class's methods
        /// </summary>
        /// <returns>a readonly enumerable of all this manager's players</returns>
        public Task<ReadOnlyCollection<Player>> GetAll() => Task.FromResult(new ReadOnlyCollection<Player>(players));

        /// <summary>
        /// update a player from <paramref name="before"/> to <paramref name="after"/>
        /// </summary>
        /// <param name="before">player to be updated</param>
        /// <param name="after">player in the state that it needs to be in after the update</param>
        /// <returns>updated player</returns>
        public Task<Player> Update(Player before, Player after)
        {
            Player[] args = { before, after };

            foreach (Player player in args)
            {
                if (player is null)
                {
                    throw new ArgumentNullException(nameof(after), "param should not be null");
                    // could also be because of before, but one param had to be chosen as an example
                    // and putting "player" there was raising a major code smell
                }
            }
            Remove(before);
            return Add(after);
        }

        /// <summary>
        /// remove a player
        /// </summary>
        /// <param name="toRemove">player to be removed</param>
        public void Remove(Player toRemove)
        {
            if (toRemove is null)
            {
                throw new ArgumentNullException(nameof(toRemove), "param should not be null");
            }
            // the built-in Remove() method will use our redefined Equals(), using Name only
            players.Remove(toRemove);
        }

        public Task<Player> GetOneByID(Guid ID)
        {
            throw new NotImplementedException();
        }
    }
}
