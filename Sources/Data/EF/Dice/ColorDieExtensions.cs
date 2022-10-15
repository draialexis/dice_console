using Data.EF.Dice.Faces;
using Model.Dice;
using Model.Dice.Faces;

namespace Data.EF.Dice
{
    public static class ColorDieExtensions
    {
        public static ColorDie ToModel(this ColorDieEntity clrDieEntity)
        {
            /*
             * creating an array of faces model
             */
            ColorFace[] faces = new ColorFace[clrDieEntity.Faces.Count - 1];
            List<ColorFace> clrFacesList = clrDieEntity.Faces.ToModels().ToList();
            clrFacesList.CopyTo(faces, 1);

            /*
             * creating the die
             */
            ColorDie die = new(clrDieEntity.Faces.ElementAt(0).ToModel(), faces);

            return die;
        }

        public static IEnumerable<ColorDie> ToModels(this IEnumerable<ColorDieEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static ColorDieEntity ToEntity(this ColorDie model)
        {
            var entity = new ColorDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(((ColorFace)face).ToEntity()); }
            return entity;
        }

        public static IEnumerable<ColorDieEntity> ToEntities(this IEnumerable<ColorDie> models)
        {
            return models.Select(model => model.ToEntity());
        }

    }
}
