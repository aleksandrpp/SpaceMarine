using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    [CreateAssetMenu(fileName = "SO_Outpost", menuName = "SpaceMarine/OutpostConfig")]
    public class OutpostConfig : ActorConfig, IFactory<Outpost>
    {
        public ActorConfig[] EnemyConfigs;
        public float HpDefault = 100;
        public int XpReward = 1;
        public float SpawnRate = 1.8f;
        public int SpawnLimit = 200;

        public Outpost Create(Vector3 position, Quaternion rotation)
        {
            var outpost = (Outpost)Instantiate(View, position, rotation);
            outpost.Bind(this);
            return outpost;
        }
    }
}