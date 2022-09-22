﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class NumberDieFace : AbstractDieFace
    {
        protected override int Value { get; }
        public NumberDieFace(int value)
        {
            Value = value;
        }

        public override object GetPracticalValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return GetPracticalValue().ToString();
        }
    }
}