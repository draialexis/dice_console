using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dice.Faces
{
    public class ImageDieFace : AbstractDieFace
    {
        /// <summary>
        /// an image URL code, to find the image URL (to find the image)
        /// </summary>
        protected override int Value { get; }

        public ImageDieFace(string uri)
        {
            /*parse an int after the last occurrence of "/" ? */
            string resultString = uri[(uri.LastIndexOf('/') + 1)..];
            /* !! here we should make sure to remove any ".jpg" etc, if there was one in the uri*/
            int result = int.Parse(resultString);
            Value = result;
        }

        public ImageDieFace(int code)
        {
            Value = code;
        }

        public override object GetPracticalValue()
        {
            return string.Format("Assets/images/{0}", Value);
        }
    }
}
