using Data.EF.Dice.Faces;
using Model.Dice.Faces;
using Model.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice
{
    public static class NumberDieExtensions
    {
        public static NumberDie ToModel(this NumberDieEntity clrDieEntity)
        {
            /*
             * creating an array of faces model
             */
            NumberFace[] faces = new NumberFace[clrDieEntity.Faces.Count - 1];
            List<NumberFace> clrFacesList = (List<NumberFace>)NumberFaceExtensions.ToModels(clrDieEntity.Faces);
            clrFacesList.CopyTo(faces, 1);

            /*
             * creating the die
             */
            NumberDie die = new(NumberFaceExtensions.ToModel(clrDieEntity.Faces.ElementAt(0)), faces);

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
