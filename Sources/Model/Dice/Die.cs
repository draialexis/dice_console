using System;
using System.Collections.Generic;
using Model.Dice.Faces;

namespace Model.Dice
{
    public class Die
    {
        private static readonly Random random = new Random();

        private List<AbstractDieFace> faces;

        public AbstractDieFace Throw()
        {
            // next(x, y) --> [x, y[
            return faces[random.Next(0, faces.Count)];
            // replace with better algo later
        }
    }


}
