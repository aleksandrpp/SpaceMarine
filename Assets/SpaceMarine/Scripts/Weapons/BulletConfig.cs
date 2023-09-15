using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    [CreateAssetMenu(fileName = "SO_Bullet", menuName = "SpaceMarine/BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        public Bullet View;
        public float OverlapRadius = .3f;
        public int OverlapCount = 10;
        public int PoolSize = 300;
        public LayerMask ObstacleLayers;
    }
}