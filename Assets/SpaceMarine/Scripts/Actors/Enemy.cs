using System.Collections.Generic;
using AK.AStar;
using AK.SpaceMarine.Parts;
using AK.SpaceMarine.Weapons;
using AK.BehaviourTree;
using UnityEngine;
using Random = UnityEngine.Random;
using Status = AK.BehaviourTree.Status;

namespace AK.SpaceMarine.Actors
{
    public class Enemy : Actor, IDamageable, IBar, IGunner, IDrop, IReward, ITracker, IPathfinder, IBlocker
    {
        [SerializeField] private Transform _bodyCenter;
        [SerializeField] private Transform _gunRoot;

        private IWorld _world;
        private IPathfinding _pathfinding;
        private EnemyConfig _config;
        private float _hp, _shootCooldown, _pathCooldown, _pathProgress;
        private Composite _tree;
        private List<Tile> _path;
        private Tile _occupationTile;
        private Vector3 _velocity;

        #region Bindings

        public override void Bind(IWorld world)
        {
            _world = world;
        }

        public void Bind(IPathfinding pathfinding)
        {
            _pathfinding = pathfinding;
        }

        public void Bind(EnemyConfig config)
        {
            _config = config;
        }

        #endregion

        #region Parts

        public GunConfig GunConfig => _config.GunConfig;
        public Gun Gun { get; private set; }
        public Transform GunRoot => _gunRoot;
        public float Range => GunConfig.Range;

        public float BarAmount => _hp / _config.HpDefault;

        public Vector3 HitPoint => _bodyCenter.position;

        public void ReceiveHit(float damage)
        {
            _hp = Mathf.Max(0, _hp - damage);
        }

        public Sprite TrackerIcon => _config.Tracker;
        public bool IsTracked => true;

        public void Drop()
        {
            if (Random.value <= _config.LootChance)
                _world.CreateActor(_config.LootConfigs[Random.Range(0, _config.LootConfigs.Length)], NearPoint(.7f), Rotation);
        }

        public void Reward(WorldData data)
        {
            data.Score = _config.XpReward;
        }

        public float BlockerPower => _config.BlockerPower;

        #endregion

        private void Start()
        {
            Gun = GunConfig.Create(_world, this);
            
            _hp = _config.HpDefault;
            _path = new List<Tile>();

            _tree = new Sequence(
                new INode[]
                {
                    new Leaf(Alive),
                    new Leaf(Reload),
                    new Leaf(CheckHero),
                    new Selector(
                        new INode[]
                        {
                            new Leaf(CheckDistance),
                            new Leaf(ChaseHero)
                        }),
                    new Leaf(EyeContact),
                    new Leaf(Attack)
                }
            );
        }

        private void FixedUpdate()
        {
            _tree.Execute();
        }

        private void Update()
        {
            var pathProgress = Mathf.RoundToInt(_pathProgress);

            if (_path != null && _path.Count > pathProgress)
                Position = Vector3.SmoothDamp(Position, _path[^pathProgress].Position, 
                    ref _velocity, .25f);
            
            TileOccupation();
        }

        private void TileOccupation()
        {
            if (!_pathfinding.Grid.TryGetNodeFromPosition(Position, out Tile tile))
                return;
            
            if (!tile.Equals(_occupationTile))
            {
                _occupationTile?.Free(this);
                _occupationTile = tile;
                _occupationTile.Occupy(this);
            }
        }
        
        public override void Dispose()
        {
            _occupationTile?.Free(this);
            base.Dispose();
        }

        #region Behaviour

        private Status Alive()
        {
            if (_hp > 0)
                return Status.Success;

            _world.Remove(this);

            return Status.Failure;
        }

        private Status CheckHero()
        {
            if (_world.Hero?.Active ?? false)
                return Status.Success;

            return Status.Failure;
        }

        private Status ChaseHero()
        {
            _pathCooldown -= Time.deltaTime;
            _pathProgress += Time.deltaTime * _config.PathSpeed;

            if (_pathCooldown < 0)
            {
                if (_pathfinding.CreatePath(Position, _world.Hero.Position, _path) == 0)
                {
                    _pathCooldown = 1 / _config.PathRate;
                    _pathProgress = 1;
                }
            }

            return Status.Failure;
        }

        private Status CheckDistance()
        {
            var direction = GetDirection(_world.Hero);
            
            if (direction.magnitude <= Gun.Config.Range)
                return Status.Success;

            return Status.Failure;
        }

        private Status EyeContact()
        {
            // if (Physics.Linecast(_bodyCenter.position, _world.Hero.Position, Physics.AllLayers, QueryTriggerInteraction.Collide))
            //     return Status.Failure;

            var direction = GetDirection(_world.Hero);
            LookRotation(direction);

            return Status.Success;
        }

        private Status Reload()
        {
            if (_shootCooldown > 0)
                _shootCooldown -= Time.deltaTime;

            return Status.Success;
        }

        private Status Attack()
        {
            if (_shootCooldown > 0)
                return Status.Failure;

            _shootCooldown = 1 / GunConfig.Rate;

            Gun.Fire();

            return Status.Success;
        }

        private Vector3 GetDirection(IPosition actor)
        {
            return actor.Position - Position;
        }

        private void LookRotation(Vector3 direction)
        {
            if (direction.sqrMagnitude > Utils.Epsilon)
                Rotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(direction).eulerAngles.y, 0));
        }

        #endregion
    }
}