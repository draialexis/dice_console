using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class ImageDieFace : AbstractDieFace<Uri>
    {
        public ImageDieFace(Uri uri):base(uri)
        {}



        public override Uri GetPracticalValue()
        {
            return Value;
        }
    }
}
