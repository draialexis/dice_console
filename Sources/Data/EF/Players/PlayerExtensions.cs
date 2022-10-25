using Model.Players;

namespace Data.EF.Players
{
    public static class PlayerExtensions
    {
        public static Player ToModel(this PlayerEntity entity) => new(name: entity.Name);

        public static IEnumerable<Player> ToModels(this IEnumerable<PlayerEntity> entities) => entities.Select(entity => entity.ToModel());

        public static PlayerEntity ToEntity(this Player model) => new() { Name = model.Name };

        public static IEnumerable<PlayerEntity> ToEntities(this IEnumerable<Player> models) => models.Select(model => model.ToEntity());
    }
}
