using AK.SpaceMarine.Parts;

namespace AK.SpaceMarine.Actors
{
    public class Loot : Actor, IPickable, IReward, ILabel
    {
        private IWorld _world;
        private LootConfig _config;
        
        #region Bindings
        
        public override void Bind(IWorld world)
        {
            _world = world;
        }

        public void Bind(LootConfig config)
        {
            _config = config;
        }
        
        #endregion

        #region Parts

        public float Range => _config.PickupRange;

        public void Pickup(IPosition position)
        {
            _world.Hero.Heal(_config.Amount);
            _world.Remove(this);
        }

        public void Reward(WorldData data)
        {
            data.Score = ((int) _config.Amount);
        }

        #endregion

        public string LabelText => $"{_config.Amount}";
    }
}