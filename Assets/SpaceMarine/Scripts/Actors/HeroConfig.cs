using AK.SpaceMarine.Parts;
using AK.SpaceMarine.Weapons;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    [CreateAssetMenu(fileName = "SO_Hero", menuName = "SpaceMarine/HeroConfig")]
    public class HeroConfig : ActorConfig, IFactory<Hero>
    {
        public GunConfig[] GunConfigs;
        public float HpDefault = 100;
        public float MoveSpeed = 5;
        public float RotationSpeed = 8;

        public Hero Create(Vector3 position, Quaternion rotation)
        {
            var hero = (Hero)Instantiate(View, position, rotation);
            hero.Bind(this);
            return hero;
        }
    }
}