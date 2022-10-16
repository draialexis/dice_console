using Data.EF.Dice.Faces;
using Model.Dice;
using Model.Dice.Faces;

namespace Data.EF.Dice
{
    public static class ColorDieExtensions
    {
        public static ColorDie ToModel(this ColorDieEntity dieEntity)
        {
            /*
             * creating an array of faces model
             */
            ColorFace[] faces = dieEntity.Faces.ToModels().ToArray();

            /*
             * creating the die
             */
            ColorDie die = new(faces[0], faces[1..]);

            return die;
        }

        public static IEnumerable<ColorDie> ToModels(this IEnumerable<ColorDieEntity> entities) => entities.Select(entity => entity.ToModel());

        public static ColorDieEntity ToEntity(this ColorDie model)
        {
            var entity = new ColorDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(((ColorFace)face).ToEntity()); }
            return entity;
        }

        public static IEnumerable<ColorDieEntity> ToEntities(this IEnumerable<ColorDie> models) => models.Select(model => model.ToEntity());
    }
}
