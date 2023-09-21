using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace AK.SpaceMarine.Weapons
{
    public class BulletPool : IObjectPool<Bullet>
    {
        private IWorld _world;
        private BulletConfig _config;
        
        private ICollection<GameObject> _objects;
        private IObjectPool<Bullet> _bullets;

        public BulletPool(IWorld world, BulletConfig config)
        {
            _world = world;
            _config = config;
            
            _objects = new HashSet<GameObject>();
            _bullets = new ObjectPool<Bullet>(CreatePooledItem, OnGet, OnRelease, OnDestroy, 
                true, config.PoolSize, config.PoolSize);
        }

        private Bullet CreatePooledItem()
        {
            var bullet = Object.Instantiate(_config.View);
            bullet.Bind(_world, _config);
            
            _objects.Add(bullet.gameObject);
            return bullet;
        }

        private void OnRelease(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnGet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnDestroy(Bullet bullet)
        {
            Object.Destroy(bullet.gameObject);
        }

        public Bullet Get()
        {
            return _bullets.Get();
        }

        public PooledObject<Bullet> Get(out Bullet bullet)
        {
            return _bullets.Get(out bullet);
        }

        public void Release(Bullet bullet)
        {
            _bullets.Release(bullet);
        }

        public void Clear()
        {
            _bullets.Clear();

            foreach (var bullet in _objects)
                Object.Destroy(bullet);
        }

        public int CountInactive => _bullets.CountInactive;
    }
}