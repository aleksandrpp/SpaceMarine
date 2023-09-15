using AK.SpaceMarine.Parts;
using AK.SpaceMarine.Weapons;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    [CreateAssetMenu(fileName = "SO_Enemy", menuName = "SpaceMarine/EnemyConfig")]
    public class EnemyConfig : ActorConfig, IFactory<Enemy>
    {
        public ActorConfig[] LootConfigs;
        public GunConfig GunConfig;
        public Sprite Tracker;
        public float LootChance = .3f;
        public float HpDefault = 100;
        public float BlockerPower = 8;
        public float PathRate = 1;
        public float PathSpeed = 8;
        public int XpReward = 1;

        public Enemy Create(Vector3 position, Quaternion rotation)
        {
            var enemy = (Enemy)Instantiate(View, position, rotation);
            enemy.Bind(this);
            return enemy;
        }
    }
}