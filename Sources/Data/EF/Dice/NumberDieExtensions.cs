using Data.EF.Dice.Faces;
using Model.Dice;
using Model.Dice.Faces;

namespace Data.EF.Dice
{
    public static class NumberDieExtensions
    {
        public static NumberDie ToModel(this NumberDieEntity dieEntity)
        {
            /*
             * creating an array of faces model
             */
            NumberFace[] faces = dieEntity.Faces.ToModels().ToArray();

            /*
             * creating the die
             */
            NumberDie die = new(faces[0], faces[1..]);

            return die;
        }

        public static IEnumerable<NumberDie> ToModels(this IEnumerable<NumberDieEntity> entities)
        {
            return entities.Select(entity => ToModel(entity));
        }

        public static NumberDieEntity ToEntity(this NumberDie model)
        {
            var entity = new NumberDieEntity();
            foreach (var face in model.Faces) { entity.Faces.Add(((NumberFace)face).ToEntity()); }
            return entity;
        }

        public static IEnumerable<NumberDieEntity> ToEntities(this IEnumerable<NumberDie> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}
