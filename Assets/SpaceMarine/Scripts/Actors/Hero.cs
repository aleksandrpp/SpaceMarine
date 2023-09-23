using System;
using AK.SpaceMarine.Parts;
using AK.SpaceMarine.Weapons;
using AK.BehaviourTree;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    public class Hero : Actor, IDamageable, IGunner, IHero, ILabel
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _bodyCenter;
        [SerializeField] private Transform _gunRoot;

        private HeroConfig _config;
        private IWorld _world;
        private IInput _input;
        
        private bool _lastBreath;
        private float _shootCooldown, _hp;
        private int _loadOutIndex;
        private Quaternion _targetRotation;
        private Vector3 _lookPosition;
        private Composite _tree;
        private IDamageable _currentEnemy;
        
        private Predicate<IPickable> _lootPredicate;
        private Predicate<IDamageable> _enemyPredicate;

        #region Bindings

        public override void Bind(IWorld world)
        {
            _world = world;
        }

        public void Bind(HeroConfig config)
        {
            _config = config;
        }

        public string Loadout { get; private set; }

        public void Bind(IInput input)
        {
            _input = input;
        }

        #endregion

        #region Parts

        public GunConfig GunConfig { get; private set; }

        public Gun Gun { get; private set; }

        public Transform GunRoot => _gunRoot;

        public float Range => GunConfig.Range;

        public bool Active => !_lastBreath;

        public Vector3 HitPoint => _bodyCenter.position;

        public void ReceiveHit(float damage)
        {
            _hp = Mathf.Max(0, _hp - damage);
        }

        public string LabelText => $"{(int) _hp}";

        public void Heal(float amount)
        {
            _hp = Mathf.Min(_config.HpDefault, _hp + amount);
        }

        #endregion

        private void Start()
        {
            _lootPredicate = pickable => (pickable.Position - Position).magnitude <= pickable.Range;
            _enemyPredicate = damageable => (damageable.Position - Position).magnitude <= Range && !damageable.Equals(Transform);

            SetLoadout();
            _input.BindLoadout(SetLoadout);

            _lookPosition = Position;
            _targetRotation = Rotation;
            _hp = _config.HpDefault;

            _tree = new Sequence(
                new INode[]
                {
                    new Selector(
                        new INode[]
                        {
                            new Leaf(Alive),
                            new Leaf(Die)
                        }
                    ),
                    new Leaf(CheckLoot),
                    new Leaf(Reload),
                    new Leaf(Follow),
                    new Leaf(GetEnemy),
                    new Leaf(TakeAim),
                    new Leaf(Shoot)
                }
            );
        }

        private void SetLoadout()
        {
            if (Gun != null)
                Destroy(Gun.gameObject);

            GunConfig = _config.GunConfigs[_loadOutIndex++ % _config.GunConfigs.Length];
            Gun = GunConfig.Create(_world, this);
            Loadout = Gun.GetType().Name;
        }

        private void Update()
        {
            Rotation = Quaternion.RotateTowards(Rotation,
                Quaternion.Euler(0, _targetRotation.eulerAngles.y, 0),
                _config.RotationSpeed * 100 * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _tree.Execute();

            var move = _input.Move * (_config.MoveSpeed * Time.deltaTime);
            var delta = Vector3.right * move.x + Vector3.forward * move.y;

            _lookPosition = Position + delta * 2;
            _controller.Move(delta + Vector3.down * .1f);
        }

        #region Behaviour

        private Status Alive()
        {
            if (_hp > 0)
                return Status.Success;

            if (_lastBreath)
                return Status.Failure;

            return Status.Failure;
        }

        private Status Die()
        {
            _lastBreath = true;
            _world.Remove(this);

            return Status.Failure;
        }

        private Status CheckLoot()
        {
            if (_world.TryGetActorFirst(_lootPredicate, out IPickable result))
            {
                result.Pickup(this);
            }

            return Status.Success;
        }

        private Status Follow()
        {
            Rotate(_lookPosition);

            return Status.Success;
        }

        private Status GetEnemy()
        {
            if (!_world.TryGetActorNearest(this, _enemyPredicate, out IDamageable result))
            {
                _currentEnemy = null;
                return Status.Failure;
            }

            _currentEnemy = result;

            Rotate(_currentEnemy.Position);

            return Status.Success;
        }

        private Status TakeAim()
        {
            if (Physics.Raycast(_bodyCenter.position, _bodyCenter.forward, out var hit,
                    Gun.Config.Range, Physics.AllLayers, QueryTriggerInteraction.Collide))
            {
                if (_currentEnemy.Equals(hit.transform))
                    return Status.Success;
            }

            return Status.Failure;
        }

        private Status Reload()
        {
            if (_shootCooldown > 0)
                _shootCooldown -= Time.deltaTime;

            return Status.Success;
        }

        private Status Shoot()
        {
            if (_shootCooldown > 0)
                return Status.Failure;

            _shootCooldown = 1 / GunConfig.Rate;

            Gun.Fire();

            return Status.Success;
        }

        private void Rotate(Vector3 targetPosition)
        {
            var direction = targetPosition - Position;

            if (direction.magnitude > Utils.Epsilon)
                _targetRotation = Quaternion.LookRotation(direction);
        }

        #endregion
    }
}