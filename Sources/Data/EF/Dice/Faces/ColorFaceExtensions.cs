﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dice.Faces;
using System.Drawing;

namespace Data.EF.Dice.Faces
{
    public static class ColorFaceExtensions
    {
        public static ColorFace ToModel(this ColorFaceEntity clrFaceEntity) => new(Color.FromArgb(clrFaceEntity.A, clrFaceEntity.R, clrFaceEntity.G, clrFaceEntity.B));

        public static IEnumerable<ColorFace> ToModels(this IEnumerable<ColorFaceEntity> entities) => entities.Select(entity => entity.ToModel());

        public static ColorFaceEntity ToEntity(this ColorFace model) => new() { A = model.Value.A, R = model.Value.R, G = model.Value.G, B = model.Value.B };

        public static IEnumerable<ColorFaceEntity> ToEntities(this IEnumerable<ColorFace> models) => models.Select(model => model.ToEntity());
    }
}
