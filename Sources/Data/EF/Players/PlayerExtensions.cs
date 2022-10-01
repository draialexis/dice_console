using Model.Players;

namespace Data.EF.Players
{
    internal static class PlayerExtensions
    {
        public static Player ToModel(this PlayerEntity entity)
        {
            return new Player(name: entity.Name);
        }

        public static IEnumerable<Player> ToModels(this IEnumerable<PlayerEntity> entities)
        {
            return entities.Select(entity => entity.ToModel());
        }

        public static PlayerEntity ToEntity(this Player model)
        {
            return new PlayerEntity() { Name = model.Name };
        }

        public static IEnumerable<PlayerEntity> ToEntities(this IEnumerable<Player> models)
        {
            return models.Select(model => model.ToEntity());
        }
    }
}
