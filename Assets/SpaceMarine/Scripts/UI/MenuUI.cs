using System;
using UnityEngine;
using UnityEngine.UI;

namespace AK.SpaceMarine.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Text _score;
        [SerializeField] private ButtonElement _playButton;
        
        private Action _onPlay;
        
        public void Bind(Action onPlay)
        {
            _onPlay = onPlay;
        }

        public void Open(UserData userData)
        {
            _score.text = $"Last Score: {userData.LastScore.ToString()}    " +
                          $"Best Score: {userData.BestScore.ToString()}";
            
            gameObject.SetActive(true);
        }
        
        private void Start()
        {
            _playButton.Bind(Play);
        }
        
        private void Play()
        {
            _onPlay?.Invoke();
            gameObject.SetActive(false);
        }
    }
}