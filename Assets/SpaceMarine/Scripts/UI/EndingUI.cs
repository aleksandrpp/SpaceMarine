using System;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class EndingUI : MonoBehaviour
    {
        [SerializeField] private Text _score;
        [SerializeField] private ButtonElement _exitButton;
        
        private Action _onExit;
        
        public void Bind(Action onExit)
        {
            _onExit = onExit;
        }

        public void Open(WorldData worldData, bool victory)
        {
            string label = victory ? "Victory" : "Lose";
            _score.text = $"{label} with Score: {worldData.Score.ToString()}";

            Open(true);
        }
        
        public void Open(bool flag)
        {
            gameObject.SetActive(flag);
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