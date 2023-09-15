using AK.SpaceMarine.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField] private float _distance = 5;
        [SerializeField] private Image _image;

        private Camera _camera;
        private float _distanceSqr;
        private IPosition _hero;
        private ITracker _actor;

        public void Bind(IPosition hero, ITracker actor)
        {
            _hero = hero;
            _actor = actor;
            _image.sprite = actor.TrackerIcon;
        }

        private void Start()
        {
            _camera = Camera.main;
            _distanceSqr = Mathf.Pow(_distance, 2);
        }

        private void LateUpdate()
        {
            transform.position = _camera.WorldToScreenPoint(GetPoint());
        }

        private Vector3 GetPoint()
        {
            var direction = _actor.Position - _hero.Position;

            return direction.sqrMagnitude <= _distanceSqr
                ? _actor.Position
                : new Ray(_hero.Position, direction).GetPoint(_distance);
        }
    }
}