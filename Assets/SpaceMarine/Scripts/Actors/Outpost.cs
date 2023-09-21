using AK.SpaceMarine.Parts;
using AK.BehaviourTree;
using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    public class Outpost : Actor, IDamageable, IBar, IReward, ILabel
    {
        [SerializeField] private Transform _bodyCenter;

        private IWorld _world;
        private OutpostConfig _config;
        private float _hp, _spawnCooldown;
        private Composite _tree;

        public override void Bind(IWorld world)
        {
            _world = world;
        }

        public void Bind(OutpostConfig config)
        {
            _config = config;
        }

        public Vector3 HitPoint => _bodyCenter.position;

        public void ReceiveHit(float damage)
        {
            _hp = Mathf.Max(0, _hp - damage);
        }

        public float BarAmount => _hp / _config.HpDefault;

        public void Reward(WorldData data)
        {
            data.Score = _config.XpReward;
        }
        
        public string LabelText => $"{(int)_hp}";

        private void Start()
        {
            _hp = _config.HpDefault;
            _tree = new Sequence(
                new INode[]
                {
                    new Leaf(Alive),
                    new Leaf(CheckHero),
                    new Leaf(SpawnEnemy)
                }
            );
        }

        private void FixedUpdate()
        {
            _tree.Execute();
        }
        
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
        
        private Status SpawnEnemy()
        {
            _spawnCooldown -= Time.deltaTime;
            
            if (_spawnCooldown <= 0 && _config.SpawnLimit > _world.GetActorCount<Enemy>())
            {
                _world.CreateActor(
                    _config.EnemyConfigs[Random.Range(0, _config.EnemyConfigs.Length)], NearPoint(1.4f), Rotation);
                
                _spawnCooldown = 1 / _config.SpawnRate;
                return Status.Success;
            }

            return Status.Failure;
        }
    }
}