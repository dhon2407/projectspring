using System;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Player
{
    public class StaminaIndicator : MonoBehaviour
    {
        [Required, SerializeField]
        private Transform barTransform = null;

        public float Value
        {
            get => _currentValue;
            set => UpdateValue(value);
        }
        
        private float _currentValue = 0f;
        private SpriteRenderer[] _sprites;
        private bool _gameStarted;

        private void Awake()
        {
            InitializeComponents();
            UpdateDisplay();

            GameManager.OnGameStarted += GameStarted;
            GameManager.OnPlayerDie += GameEnded;
        }

        private void GameEnded()
        {
            FadeOut();
            _gameStarted = false;
        }

        private void GameStarted()
        {
            _gameStarted = true;
        }

        private void InitializeComponents()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        private void FadeOut()
        {
            foreach (var sprite in _sprites)
            {
                sprite.DOKill();
                sprite.DOFade(0, 0.2f);
            }
        }

        private void FadeIn()
        {
            if (!_gameStarted)
                return;
            
            foreach (var sprite in _sprites)
            {
                sprite.DOKill();
                sprite.DOFade(1, 0.1f);
            }
        }

        private void UpdateValue(float targetValue)
        {
            if (Math.Abs(targetValue - _currentValue) < 0.01f)
                return;
            
            DOTween.Kill(this);
            FadeIn();
            
            targetValue = Mathf.Clamp01(targetValue);
            var speed = targetValue > _currentValue ? 0.5f: 0.15f;
            
            DOTween.To(() => _currentValue, newValue => _currentValue = newValue, targetValue, speed)
                .SetEase(Ease.OutCubic)
                .SetTarget(this)
                .OnUpdate(UpdateDisplay);
        }

        private void UpdateDisplay()
        {
            var updatedScale = barTransform.localScale;
            updatedScale.x = _currentValue;
            barTransform.localScale = updatedScale;

            if (Math.Abs(_currentValue - 1) < 0.01f)
                FadeOut();
        }
    }
}
