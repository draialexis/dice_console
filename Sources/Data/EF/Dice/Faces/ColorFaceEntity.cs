using System.Drawing;

namespace Data.EF.Dice.Faces
{
    public class ColorFaceEntity : FaceEntity
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Guid ColorDieEntityID { get; set; }
        public ColorDieEntity ColorDieEntity { get; set; }

        public void SetValue(Color c)
        {
            A = c.A;
            R = c.R;
            G = c.G;
            B = c.B;
        }
    }
}
