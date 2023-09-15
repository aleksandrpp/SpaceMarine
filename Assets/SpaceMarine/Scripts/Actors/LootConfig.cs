using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    [CreateAssetMenu(fileName = "SO_Loot", menuName = "SpaceMarine/LootConfig")]
    public class LootConfig : ActorConfig, IFactory<Loot>
    {
        public float PickupRange = 1.5f;
        public float Amount = 100;

        public Loot Create(Vector3 position, Quaternion rotation)
        {
            var loot = (Loot)Instantiate(View, position, rotation);
            loot.Bind(this);
            return loot;
        }
    }
}