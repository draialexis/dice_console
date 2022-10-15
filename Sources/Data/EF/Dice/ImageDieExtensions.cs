using Data.EF.Dice.Faces;
using Model.Dice.Faces;
using Model.Dice;

namespace Data.EF.Dice
{
    public static class ImageDieExtensions
    {
        public static ImageDie ToModel(this ImageDieEntity clrDieEntity)
        {
            /*
             * creating an array of faces model
             */
            ImageFace[] faces = new ImageFace[clrDieEntity.Faces.Count - 1];
            List<ImageFace> clrFacesList = (List<ImageFace>)ImageFaceExtensions.ToModels(clrDieEntity.Faces);
            clrFacesList.CopyTo(faces, 1);


            /*
             * creating the die
             */
            ImageDie die = new( ImageFaceExtensions.ToModel(clrDieEntity.Faces.ElementAt(0)), faces);

            return die;
        }

        public static IEnumerable<ImageDie> ToModels(this IEnumerable<ImageDieEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static ImageDieEntity ToEntity(this ImageDie model)
        {
            var entity = new ImageDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(ImageFaceExtensions.ToEntity((ImageFace)face)); }
            return entity;
        }

        public static IEnumerable<ImageDieEntity> ToEntities(this IEnumerable<ImageDie> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}
