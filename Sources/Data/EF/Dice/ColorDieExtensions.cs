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
            ColorFace[] faces= new ColorFace[clrDieEntity.Faces.Count-1];
            List<ColorFace> clrFacesList = (List<ColorFace>)ColorFaceExtensions.ToModels(clrDieEntity.Faces);
            clrFacesList.CopyTo(faces, 1);

            /*
             * creating the die
             */
            ColorDie die = new (ColorFaceExtensions.ToModel(clrDieEntity.Faces.ElementAt(0)), faces);
            
            return die;
        }

        public static IEnumerable<ColorDie> ToModels(this IEnumerable<ColorDieEntity> entities)
        {
            return entities.Select(entity => ToModel(entity));
        }

        public static ColorDieEntity ToEntity(this ColorDie model)
        {
            var entity = new ColorDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(ColorFaceExtensions.ToEntity((ColorFace)face)); }
            return entity;
        }

        public static IEnumerable<ColorFaceEntity> ToEntities(this IEnumerable<ColorFace> models)
        {
            return models.Select(model => model.ToEntity());
        }

    }
}
