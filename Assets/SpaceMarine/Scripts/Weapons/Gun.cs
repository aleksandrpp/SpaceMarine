using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    [DisallowMultipleComponent]
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Transform _muzzleRoot;

        public GunConfig Config { get; private set; }

        protected IGunner Gunner { get; private set; }
        protected IWorld World { get; private set; }

        public void Bind(IWorld world, GunConfig config, IGunner gunner)
        {
            World = world;
            Config = config;
            Gunner = gunner;
        }

        public abstract void Fire();

        protected void Launch(Vector3 position, Quaternion rotation, Vector3 direction)
        {
            if (!World.Bullets.ContainsKey(Config.BulletConfig))
            {
                World.Bullets.Add(Config.BulletConfig, new BulletPool(World, Config.BulletConfig));
            }

            World.Bullets[Config.BulletConfig]
                .Get()
                .Launch(position, rotation, Gunner, direction, Config.Range, Config.Speed, Config.Perks);
        }
    }
}