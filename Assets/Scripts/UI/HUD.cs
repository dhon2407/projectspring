using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            GameManager.OnGameStarted += FadeIn;
            GameManager.OnGameEnded += FadeOut;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= FadeIn;
            GameManager.OnGameEnded -= FadeOut;
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