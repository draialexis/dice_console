using System;
using System.Drawing;

namespace Data.EF.Dice.Faces
{
    public class ColorFaceEntity
    {
        public Guid ID { get; set; }

        public string Value { get; set; }

        public void SetValue(Color c)
        {
            Value = c.ToString();
        }
    }
}
