using Managers;
using UnityEngine;

namespace UI
{
    public class TapToStart : MonoBehaviour
    {
        private bool _gameStarted;

        private void Awake()
        {
            GameManager.OnGameStarted += GameStart;
            GameManager.OnReplayGame += ReplayGame;
            Options.OnOpen += OnOptionsOpen;
        }

        private void OnOptionsOpen(bool isOpen)
        {
            if (_gameStarted)
                return;
            
            gameObject.SetActive(isOpen == false);
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= GameStart;
            GameManager.OnReplayGame -= ReplayGame;

        }

        private void GameStart()
        {
            _gameStarted = true;
            gameObject.SetActive(false);
        }
        
        private void ReplayGame()
        {
            _gameStarted = false;
            gameObject.SetActive(true);
        }
    }
}
