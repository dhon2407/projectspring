using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class EndGameDistanceHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI num = null;

        private float _currentValue;

        private void Awake()
        {
            Hide();
        }

        public void Show(int distanceTraveled, UnityAction onFinish = null)
        {
            transform.DOScale(1, 0.5f).OnComplete(() => AnimateValue(distanceTraveled, onFinish));
        }

        private void AnimateValue(int distanceTraveled, UnityAction onFinish)
        {
            _currentValue = 0;

            DOTween.To(() => _currentValue, value => _currentValue = value, distanceTraveled, 2f)
                .SetEase(Ease.OutQuad)
                .OnUpdate(UpdateValue)
                .OnComplete(onFinish.Invoke);
        }

        private void UpdateValue()
        {
            num.text = _currentValue.ToString("0");
        }

        public void Hide()
        {
            transform.DOKill();
            transform.DOScale(0, 0f).OnComplete(() =>
            {
                _currentValue = 0;
                UpdateValue();
            });
        }
    }
}
