using DG.Tweening;
using Level;
using Managers;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private GameObject optionsButton;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            GameManager.OnGameStarted += FadeIn;
            GameManager.OnGameStarted += HideSettingsButton;
            GameManager.OnGameEnded += FadeOut;
            GameManager.OnGameEnded += HideSettingsButton;
            GameManager.OnReplayGameReadyIn += ShowSettingsButton;
            TutorialHandler.OnTutorialFinished += (sender, args) => ShowSettingsButton(0);
        }

        private void ShowSettingsButton(float value)
        {
            optionsButton.SetActive(true);
        }

        private void HideSettingsButton()
        {
            optionsButton.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= FadeIn;
            GameManager.OnGameEnded -= FadeOut;
            GameManager.OnGameStarted -= HideSettingsButton;
            GameManager.OnGameEnded -= HideSettingsButton;
            GameManager.OnReplayGameReadyIn -= ShowSettingsButton;
        }

        private void FadeIn()
        {
            _canvasGroup.DOFade(1, 0.5f);
        }

        private void FadeOut()
        {
            _canvasGroup.DOFade(0, 0.5f);
        }
    }
}