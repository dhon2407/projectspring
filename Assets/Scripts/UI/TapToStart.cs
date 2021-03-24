using Managers;
using UnityEngine;

namespace UI
{
    public class TapToStart : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.OnGameStarted += GameStart;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= GameStart;
        }

        private void GameStart()
        {
            gameObject.SetActive(false);
        }
    }
}
