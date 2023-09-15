using System.Threading.Tasks;
using AK.AStar;
using AK.SpaceMarine.Actors;
using AK.SpaceMarine.Maze;
using AK.SpaceMarine.Parts;
using AK.SpaceMarine.UI;
using Cinemachine;
using UnityEngine;

namespace AK.SpaceMarine
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        [SerializeField] private MazeConfig _mazeConfig;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private WorldUI _worldUI;
        [SerializeField] private MenuUI _menuUI;
        [SerializeField] private PauseUI _pauseUI;
        [SerializeField] private EndingUI _endingUI;

        private IWorld _world;
        private IMaze _maze;
        private IGrid _grid;
        private IPathfinding _pathfinding;
        private UserData _userData;
        private bool _pauseState;

        public void Start()
        {
            _userData = new UserData();
            _userData.LoadFromFile();
            
            _menuUI.Bind(Play);
            _pauseUI.Bind((() => { Clear(); Exit();}));
            _endingUI.Bind(Exit);

            _menuUI.Open(_userData);
            _worldUI.Open(false);
            _pauseUI.Open(false);
            _endingUI.Open(false);
        }

        private async void Play()
        {
            _maze = new WfcMaze(_mazeConfig);
            uint mazeSeed = (uint) Random.Range(1, int.MaxValue);
            _maze.Build(mazeSeed);

            await Task.Delay(1000);

            _grid = new PathGrid(_gridConfig);
            _grid.Build();

            _pathfinding = new Pathfinding(_grid);

            _world = new World(OnActor);
            _world.CreateFromTags<Tag>();

            _worldUI.Bind(_world);
            _worldUI.Open(true);
            
            _input.BindPause(Pause);
        }

        private void Pause()
        {
            _pauseState = !_pauseState;
            _pauseUI.Open(_pauseState);
        }

        private void OnActor(Actor actor, bool created)
        {
            if (created)
                ActorCreated(actor);
            else
                ActorRemoved(actor);
        }

        private void ActorCreated(Actor actor)
        {
            if (actor is IHero hero)
            {
                _world.Hero = hero;
                _world.Hero.Bind(_input);
                _targetGroup.AddMember(_world.Hero.Transform, 1, 1);
            }

            if (actor is IPathfinder npc)
                npc.Bind(_pathfinding);
        }

        private void ActorRemoved(Actor actor)
        {
            if (actor is IHero)
            {
                _endingUI.Open(_world.Data, false);
                Clear();
            }

            if (actor is Enemy)
            {
                if (_world.GetActorCount<Enemy>() == 1)
                {
                    _endingUI.Open(_world.Data, true);
                    Clear();
                }
            }
        }

        private void OnDrawGizmos()
        {
            _grid?.Debug();
        }

        private void Clear()
        {
            _targetGroup.RemoveMember(_world.Hero.Transform);
            _input.BindPause(null);
            _world.SaveData(_userData);
            _world.Dispose();
            _worldUI.Open(false);
        }

        private void Exit()
        {
            _maze.Dispose();
            _menuUI.Open(_userData);
        }
    }
}