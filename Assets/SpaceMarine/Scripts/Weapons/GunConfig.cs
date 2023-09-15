using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    [CreateAssetMenu(fileName = "SO_Gun", menuName = "SpaceMarine/GunConfig")]
    public class GunConfig : ScriptableObject
    {
        public Gun View;
        public BulletConfig BulletConfig;
        public Perks Perks;
        public float Range = 10;
        public float Damage = 5;
        public float Rate = .2f;
        public float Speed = 10;

        public Gun Create(IWorld world, IGunner gunner)
        {
            var gun = Instantiate(gunner.GunConfig.View, gunner.GunRoot);
            gun.Bind(world, this, gunner);
            return gun;
        }
    }
}