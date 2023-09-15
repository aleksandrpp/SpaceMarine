using System;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private ButtonElement _exitButton;
        
        private Action _onExit;
        
        public void Bind(Action onExit)
        {
            _onExit = onExit;
        }

        public void Open(bool flag)
        {
            gameObject.SetActive(flag);
            Time.timeScale = flag ? 0 : 1;
        }
        
        private void Start()
        {
            _exitButton.Bind(Exit);
        }
        
        private void Exit()
        {
            _onExit?.Invoke();
            Open(false);
        }
    }
}