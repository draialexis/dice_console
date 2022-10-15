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

        public static Turn ToModel(this TurnEntity entity)
        {

            Dictionary<Die, Face> DiceNFaces = new();

            DiceNFaces = Utils.Enumerables.FeedListsToDict(
                DiceNFaces,
                entity.ColorDice.ToModels() as List<Die>,
                entity.ColorFaces.ToModels() as List<Face>
                );

            DiceNFaces = Utils.Enumerables.FeedListsToDict(
                DiceNFaces,
                entity.NumberDice.ToModels() as List<Die>,
                entity.NumberFaces.ToModels() as List<Face>
                );

            DiceNFaces = Utils.Enumerables.FeedListsToDict(
                DiceNFaces,
                entity.ImageDice.ToModels() as List<Die>,
                entity.ImageFaces.ToModels() as List<Face>
                );

            return Turn.CreateWithSpecifiedTime(
                when: entity.When,
                player: entity.Player.ToModel(),
                diceNFaces: DiceNFaces
            );
        }

        public static IEnumerable<Turn> ToModels(this IEnumerable<TurnEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static TurnEntity ToEntity(this Turn model)
        {

            List<NumberDieEntity> NumberDiceEntities = new();
            List<ColorDieEntity> ColorDiceEntities = new();
            List<ImageDieEntity> ImageDiceEntities = new();
            List<NumberFaceEntity> NumberFaceEntities = new();
            List<ColorFaceEntity> ColorFaceEntities = new();
            List<ImageFaceEntity> ImageFaceEntities = new();

            foreach (KeyValuePair<Die, Face> kvp in model.DiceNFaces)
            {
                if (kvp.Key.GetType() == typeof(NumberDie))
                {
                    NumberDiceEntities.Add((kvp.Key as NumberDie).ToEntity());
                    NumberFaceEntities.Add((kvp.Value as NumberFace).ToEntity());
                }
                if (kvp.Key.GetType() == typeof(ImageDie))
                {
                    ImageDiceEntities.Add((kvp.Key as ImageDie).ToEntity());
                    ImageFaceEntities.Add((kvp.Value as ImageFace).ToEntity());
                }
                if (kvp.Key.GetType() == typeof(ColorDie))
                {
                    ColorDiceEntities.Add((kvp.Key as ColorDie).ToEntity());
                    ColorFaceEntities.Add((kvp.Value as ColorFace).ToEntity());
                }
            }

            return new TurnEntity()
            {
                When = model.When,
                Player = model.Player.ToEntity(),
                NumberDice = NumberDiceEntities,
                NumberFaces = NumberFaceEntities,
                ColorDice = ColorDiceEntities,
                ColorFaces = ColorFaceEntities,
                ImageDice = ImageDiceEntities,
                ImageFaces = ImageFaceEntities
            };
        }

        public static IEnumerable<TurnEntity> ToEntities(this IEnumerable<Turn> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}