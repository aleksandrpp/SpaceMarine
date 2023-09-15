using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    [RequireComponent(typeof(Image))]
    public class ButtonElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image image;
        
        private Action _onClickAction;
        private bool _disabled;

        private void OnValidate()
        {
            image = GetComponent<Image>();
        }

        public void Bind(Action onClickAction)
        {
            _onClickAction = onClickAction;
        }

        public void Enable(bool flag)
        {
            _disabled = !flag;
            image.color = flag ? Color.white : Color.gray;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_disabled) return;
            _onClickAction?.Invoke();
        }
    }
}