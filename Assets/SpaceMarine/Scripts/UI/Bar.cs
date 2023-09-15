using AK.SpaceMarine.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Gradient _gradient;

        private Camera _camera;
        private IBar _actor;

        public void Bind(IBar actor)
        {
            _actor = actor;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.position = _camera.WorldToScreenPoint(_actor.Position);
        }

        private void FixedUpdate()
        {
            _image.color = _gradient.Evaluate(_actor.BarAmount);
            _image.fillAmount = _actor.BarAmount;
        }
    }
}