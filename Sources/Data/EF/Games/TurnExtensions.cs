using Data.EF.Dice;
using Data.EF.Dice.Faces;
using Data.EF.Players;
using Model.Dice;
using Model.Dice.Faces;
using Model.Games;

namespace Data.EF.Games
{
    public static class TurnExtensions
    {

        private static (List<Die>, List<Face>) ToModels(ICollection<DieEntity> diceEntities, ICollection<FaceEntity> faceEntities)
        {
            List<Die> dice = new();
            List<Face> faces = new();

            foreach (DieEntity dieEntity in diceEntities)
            {
                if (dieEntity.GetType() == typeof(NumberDieEntity)) { dice.Add((dieEntity as NumberDieEntity).ToModel()); }
                if (dieEntity.GetType() == typeof(ColorDieEntity)) { dice.Add((dieEntity as ColorDieEntity).ToModel()); }
                if (dieEntity.GetType() == typeof(ImageDieEntity)) { dice.Add((dieEntity as ImageDieEntity).ToModel()); }
            }
            foreach (FaceEntity faceEntity in faceEntities)
            {
                if (faceEntity.GetType() == typeof(NumberFaceEntity)) { faces.Add((faceEntity as NumberFaceEntity).ToModel()); }
                if (faceEntity.GetType() == typeof(ColorFaceEntity)) { faces.Add((faceEntity as ColorFaceEntity).ToModel()); }
                if (faceEntity.GetType() == typeof(ImageFaceEntity)) { faces.Add((faceEntity as ImageFaceEntity).ToModel()); }
            }

            return (dice, faces);
        }

        public static Turn ToModel(this TurnEntity entity)
        {
            Dictionary<Die, Face> DiceNFaces = new();

            List<Die> keysList;
            List<Face> valuesList;

            (keysList, valuesList) = ToModels(entity.Dice, entity.Faces);

            DiceNFaces = Utils.Enumerables.FeedListsToDict(DiceNFaces, keysList, valuesList);

            return Turn.CreateWithSpecifiedTime(when: entity.When, player: entity.PlayerEntity.ToModel(), diceNFaces: DiceNFaces);
        }

        public static IEnumerable<Turn> ToModels(this IEnumerable<TurnEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static TurnEntity ToEntity(this Turn model)
        {

            List<DieEntity> DiceEntities = new();
            List<FaceEntity> FaceEntities = new();

            foreach (KeyValuePair<Die, Face> kvp in model.DiceNFaces)
            {
                if (kvp.Key.GetType() == typeof(NumberDie)) { DiceEntities.Add((kvp.Key as NumberDie).ToEntity()); FaceEntities.Add((kvp.Value as NumberFace).ToEntity()); }
                if (kvp.Key.GetType() == typeof(ImageDie)) { DiceEntities.Add((kvp.Key as ImageDie).ToEntity()); FaceEntities.Add((kvp.Value as ImageFace).ToEntity()); }
                if (kvp.Key.GetType() == typeof(ColorDie)) { DiceEntities.Add((kvp.Key as ColorDie).ToEntity()); FaceEntities.Add((kvp.Value as ColorFace).ToEntity()); }
            }

            return new TurnEntity() { When = model.When, PlayerEntity = model.Player.ToEntity(), Dice = DiceEntities, Faces = FaceEntities };
        }

        public static IEnumerable<TurnEntity> ToEntities(this IEnumerable<Turn> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}