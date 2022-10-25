namespace Data.EF.Dice.Faces
{
    public class ImageFaceEntity : FaceEntity
    {
        public string Value { get; set; }
        public Guid ImageDieEntityID { get; set; }
        public ImageDieEntity ImageDieEntity { get; set; }
    }
}
