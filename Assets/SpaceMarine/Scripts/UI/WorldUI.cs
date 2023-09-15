using AK.SpaceMarine.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private UIConfig _config;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Text _score, _loadout;

        private TrackerWidget _trackers;
        private LabelWidget _labels;
        private BarWidget _bars;
        private IWorld _world;

        public void Bind(IWorld world)
        {
            _world = world;
        }

        public void Open(bool flag)
        {
            gameObject.SetActive(flag);
        }

        private void Start()
        {
            _trackers = new TrackerWidget(_canvas, _config.TrackerView);
            _labels = new LabelWidget(_canvas, _config.LabelView);
            _bars = new BarWidget(_canvas, _config.BarView);
        }

        private void Update()
        {
            foreach (var actor in _world.Actors)
            {
                if (actor is ITracker tracker)
                    _trackers.Update(tracker, _world.Hero);
                
                if (actor is IBar bar)
                    _bars.Update(bar);
                
                if (actor is ILabel label)
                    _labels.Update(label);
            }
            
            _trackers.Cleanup();
            _bars.Cleanup();
            _labels.Cleanup();
        }

        private void FixedUpdate()
        {
            Score();
        }

        private void Score()
        {
            _score.text = $"Current Score: {_world.Data.Score.ToString()}";
            _loadout.text = $"[E] {_world.Hero.Loadout}";
        }

        private void OnDisable()
        {
            _trackers?.Disable();
            _bars?.Disable();
            _labels?.Disable();
        }
    }
}