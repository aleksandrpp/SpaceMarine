using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AK.SpaceMarine
{
    public interface IInput
    {
        Vector2 Move { get; }

        void BindPause(Action onPause);
        
        void BindLoadout(Action onLoadout);
    }
    
    public class Input : MonoBehaviour, IInput
    {
        [SerializeField] private PlayerInput _input;

        private InputAction _move;
        
        public Vector2 Move => _move.ReadValue<Vector2>();
        
        public void BindPause(Action onPause)
        {
            _onPause = onPause;
        }

        public void BindLoadout(Action onLoadout)
        {
            _onLoadout = onLoadout;
        }

        private InputAction 
            _pause, 
            _loadout;

        private Action 
            _onPause, 
            _onLoadout;

        private void Awake()
        {
            _move = _input.actions["Move"];
            _pause = _input.actions["Pause"];
            _loadout = _input.actions["Loadout"];
        }

        private void Update()
        {
            if (_pause.WasPressedThisFrame())
                _onPause?.Invoke();
            
            if (_loadout.WasPressedThisFrame())
                _onLoadout?.Invoke();
        }
    }
}
