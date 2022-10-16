using Data.EF.Dice.Faces;
using Model.Dice;
using Model.Dice.Faces;

namespace Data.EF.Dice
{
    public static class ImageDieExtensions
    {
        public static ImageDie ToModel(this ImageDieEntity dieEntity)
        {
            /*
             * creating an array of faces model
             */
            ImageFace[] faces = dieEntity.Faces.ToModels().ToArray();

            /*
             * creating the die
             */
            ImageDie die = new(faces[0], faces[1..]);

            return die;
        }

        public static IEnumerable<ImageDie> ToModels(this IEnumerable<ImageDieEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static ImageDieEntity ToEntity(this ImageDie model)
        {
            var entity = new ImageDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(((ImageFace)face).ToEntity()); }
            return entity;
        }

        public static IEnumerable<ImageDieEntity> ToEntities(this IEnumerable<ImageDie> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}
