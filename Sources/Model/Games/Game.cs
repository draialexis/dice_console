﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Games
{
    public class Game
    {
        public string Name { get; private set; }

        private readonly IEnumerable<Turn> turns = new List<Turn>();

        public Game(string name)
        {
            Name = name;
        }
    }
}
