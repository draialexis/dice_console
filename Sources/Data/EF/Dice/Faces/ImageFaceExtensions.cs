using Data.EF.Players;
using Model.Dice.Faces;
using Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF.Dice.Faces
{
    public static class ImageFaceExtensions
    {
        public static ImageFace ToModel(this ImageFaceEntity entity) => new(new Uri(entity.Value));

        public static IEnumerable<ImageFace> ToModels(this IEnumerable<ImageFaceEntity> entities) => entities.Select(entity => entity.ToModel());

        public static ImageFaceEntity ToEntity(this ImageFace model) => new() { Value = model.StringValue };

        public static IEnumerable<ImageFaceEntity> ToEntities(this IEnumerable<ImageFace> models) => models.Select(model => model.ToEntity());
    }
}
