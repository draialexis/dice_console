namespace Data.EF.Dice.Faces
{
    public class NumberFaceEntity : FaceEntity
    {
        public int Value { get; set; }
        public Guid NumberDieEntityID { get; set; }
        public NumberDieEntity NumberDieEntity { get; set; }
    }
}
