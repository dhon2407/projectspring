using System;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utilities.Helpers;

namespace UI
{
    [HideMonoScript]
    public class NameInput : MonoBehaviour
    {
        [SerializeField]
        private GameObject acceptButton = null;
        [SerializeField]
        private TMP_InputField inputField = null;

        private CanvasGroup _canvasGroup;

        public void OnAcceptName()
        {
            FadeOut(0.3f, () => GameManager.SetUsername(inputField.text));
        }

        private void FadeOut(float duration, Action action)
        {
            _canvasGroup.DOFade(0, duration)
                .OnComplete(() =>
                {
                    action?.Invoke();
                    Destroy(gameObject);
                });
        }

        private void OnDestroy()
        {
            _canvasGroup.DOKill();
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            acceptButton.SetActive(false);
            inputField.characterLimit = 6;
            inputField.onValueChanged.AddListener(CheckName);

            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Start()
        {
            Action selectInput = inputField.Select;
            selectInput.DelayInvoke(2);
        }

        private void CheckName(string playerName)
        {
            acceptButton.SetActive(playerName.Length > 1);
        }
    }
}
