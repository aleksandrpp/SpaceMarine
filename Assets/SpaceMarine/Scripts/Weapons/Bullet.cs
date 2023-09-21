using System.Collections.Generic;
using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    [DisallowMultipleComponent]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _trailParticle;

        private BulletConfig _config;
        private Collider[] _overlapColliders;
        private IGunner _gunner;
        private IWorld _world;
        private float _speed, _range, _lifetimeProgress;
        private Vector3 _force, _lastPosition, _direction;
        private Perks _flags;
        private List<IDamageable> _ignore;

        public void Bind(IWorld world, BulletConfig config)
        {
            _world = world;
            _config = config;
            
            _overlapColliders = new Collider[_config.OverlapCount];
            _ignore = new List<IDamageable>();
        }

        public void Launch(Vector3 position, Quaternion rotation, IGunner gunner, Vector3 direction, float range, float speed, Perks flags, List<IDamageable> ignore = null)
        {
            transform.SetPositionAndRotation(position, rotation);
            Trail(true);
            
            _gunner = gunner;
            _lastPosition = position;
            _direction = direction;
            _flags = flags;
            _range = range;
            _speed = speed;
            _force = direction * speed;
            _lifetimeProgress = range / speed;

            if (ignore == null)
            {
                _ignore.Clear();
                _ignore.Add(_gunner as IDamageable);
            }
        }

        private void Update()
        {
            if (Time.deltaTime > 0)
                Verlet();

            _lifetimeProgress -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (Overlap() || _lifetimeProgress <= 0 || transform.position.y < -1)
                ReleaseBullet();
        }

        private void Verlet()
        {
            var position = transform.position;
            var targetPosition = position + (position - _lastPosition) + _force * Time.deltaTime;

            _lastPosition = position;
            transform.position = targetPosition;

            Vector3 direction = targetPosition - _lastPosition;
            if (direction.sqrMagnitude > Utils.Epsilon)
                transform.forward = direction;

            _force = Vector3.zero;
        }

        private bool Overlap()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, _config.OverlapRadius, _overlapColliders);
            for (int i = 0; i < count; i++)
            {
                var t = _overlapColliders[i].transform;

                if (_config.ObstacleLayers.Contains(t.gameObject.layer))
                {
                    Bounce(_overlapColliders[i]);
                    return true;
                }

                if (_world.TryGetActorFirst(Damageable, out IDamageable result))
                {
                    Hit(result);
                    Ricochet(result.HitPoint, t);
                    return true;
                }

                bool Damageable(IDamageable actor)
                {
                    return !_ignore.Contains(actor) && actor.Equals(t);
                }
            }

            return false;
        }

        private void Bounce(Collider overlapCollider)
        {
            if (!UsePerk(ref _flags, Perks.Bounce))
                return;

            var hitPoint = overlapCollider.ClosestPoint(transform.position);
            if (!Physics.Raycast(hitPoint - _direction, _direction, out RaycastHit hit))
                return;

            GetBullet(transform.position, Vector3.Reflect(_direction, hit.normal));
        }

        private void Hit(IDamageable damageable)
        {
            damageable.ReceiveHit(_gunner.GunConfig.Damage);
            _ignore.Add(damageable);
        }

        private void Ricochet(Vector3 hitPoint, Transform hitTransform)
        {
            if (!UsePerk(ref _flags, Perks.Ricochet))
                return;

            if (!_world.TryGetActorNearest(new CustomRange(hitPoint, _range), Damageable, out IDamageable closeEnemy))
                return;

            bool Damageable(IDamageable actor)
            {
                return !_ignore.Contains(actor) && !actor.Equals(hitTransform);
            }

            GetBullet(hitPoint, (closeEnemy.HitPoint - hitPoint).normalized);
        }
        
        private void GetBullet(Vector3 position, Vector3 direction)
        {
            _world.Bullets[_config]
                .Get()
                .Launch(position, Quaternion.Euler(direction), _gunner, direction, _range, _speed, _flags, _ignore);
        }

        private void ReleaseBullet()
        {
            Trail(false);
            _world.Bullets[_config].Release(this);
        }

        private void Trail(bool flag)
        {
            if (_trailParticle == null) return;
            
            if (flag)
                _trailParticle.Play(true);
            else
                _trailParticle.Stop(true);
        }

        private static bool UsePerk(ref Perks flag, Perks perk)
        {
            if ((flag & perk) == 0)
                return false;

            flag ^= perk;
            return true;
        }
    }
}