﻿using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class NumberDie : AbstractDie<AbstractDieFace>
    {
        public NumberDie(params NumberDieFace[] faces) : base(faces)
        {
        }
    }
}
