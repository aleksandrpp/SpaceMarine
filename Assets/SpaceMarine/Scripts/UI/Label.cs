using AK.SpaceMarine.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class Label : MonoBehaviour
    {
        [SerializeField] private Text _label;

        private Camera _camera;
        private ILabel _actor;

        public void Bind(ILabel actor)
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
            _label.text = _actor.LabelText;
        }
    }
}