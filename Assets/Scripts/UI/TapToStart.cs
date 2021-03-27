using Managers;
using UnityEngine;

namespace UI
{
    public class TapToStart : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.OnGameStarted += GameStart;
            GameManager.OnReplayGame += ReplayGame;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= GameStart;
            GameManager.OnReplayGame -= ReplayGame;

        }

        private void GameStart()
        {
            gameObject.SetActive(false);
        }
        
        private void ReplayGame()
        {
            gameObject.SetActive(true);
        }
    }
}
