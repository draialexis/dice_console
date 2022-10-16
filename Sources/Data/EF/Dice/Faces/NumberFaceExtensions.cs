using Model.Dice.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice.Faces
{
    public static class NumberFaceExtensions
    {
        public static NumberFace ToModel(this NumberFaceEntity entity) => new(entity.Value);

        public static IEnumerable<NumberFace> ToModels(this IEnumerable<NumberFaceEntity> entities) => entities.Select(entity => entity.ToModel());

        public static NumberFaceEntity ToEntity(this NumberFace model) => new() { Value = model.Value };

        public static IEnumerable<NumberFaceEntity> ToEntities(this IEnumerable<NumberFace> models) => models.Select(model => model.ToEntity());
    }
}
