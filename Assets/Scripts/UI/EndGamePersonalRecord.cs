using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EndGamePersonalRecord : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI num = null;
        [SerializeField]
        private TextMeshProUGUI message = null;
        [SerializeField]
        private DOTweenAnimation messageAnimator = null;

        private float _currentValue;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Show(int value, int record)
        {
            var newRecord = value > record;
            message.text = newRecord ? "New record!" : "Personal best!";
            _currentValue = newRecord ? value : record;
            UpdateValue();
            
            if (newRecord)
                messageAnimator.DOPlay();

            _canvasGroup.DOFade(1, 0.3f);
        }

        private void UpdateValue()
        {
            num.text = _currentValue.ToString("0");
        }

        public void Hide()
        {
            _canvasGroup.DOFade(0, 0f)
                .OnComplete(() =>
                {
                    messageAnimator.DORewind();
                    _currentValue = 0;
                    UpdateValue();
                });
        }
    }
}