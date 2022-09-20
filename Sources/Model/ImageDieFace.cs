using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ImageDieFace : IDieFace
    {
        private string ImageUrlCode;

        public ImageDieFace(string v)
        {
            this.ImageUrlCode = v;
        }

        public string getImageUrlCode() { return ImageUrlCode; }
        public void setImageUrlCode(string ImageUrlCode) { this.ImageUrlCode = ImageUrlCode; }
    }
}
