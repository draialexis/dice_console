﻿using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice
{
    public class ImageDie : AbstractDie<ImageDieFace>
    {

        public ImageDie(string name, params ImageDieFace[] faces) : base(name, faces)
        {
        }

    }
}