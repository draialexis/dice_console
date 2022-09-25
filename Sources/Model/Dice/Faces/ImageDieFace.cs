using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class ImageDieFace : AbstractDieFace
    {
        public ImageDieFace(string uri)
        {
            Value = int.Parse(Path.GetFileNameWithoutExtension(uri));
        }

        public ImageDieFace(int code)
        {
            Value = code;
        }

        public override object GetPracticalValue()
        {
            return string.Format($"Assets/images/{Value}.png");
        }
    }
}
