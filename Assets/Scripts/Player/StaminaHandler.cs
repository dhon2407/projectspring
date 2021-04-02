using System;
using System.Collections;
using Managers;
using MEC;
using UI.Player;
using UnityEngine;
using Utilities.Helpers;

namespace Player
{
    public class StaminaHandler : MonoBehaviour
    {
        private float _maxStamina;
        private float _currentStamina = 100;
        private StaminaIndicator _indicator;
        private bool _recoverySuspend;
        
        private float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                if (_currentStamina > value)
                    SuspendRecovery();
                
                _currentStamina = value;
                if (_indicator != null)
                    _indicator.Value = _currentStamina / _maxStamina;
            }
        }

        public void Recover(float value)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina + value, 0, _maxStamina);
        }

        public bool Consume(int needed)
        {
            if (HasStamina(needed))
            {
                CurrentStamina = Mathf.Clamp(CurrentStamina - needed, 0, _maxStamina);
                return true;
            }

            return false;
        }
        
        public bool HasStamina(int needed)
        {
            return CurrentStamina >= needed;
        }
        
        private void SuspendRecovery()
        {
            Timing.KillCoroutines(nameof(SuspendRecovery));
            _recoverySuspend = true;
            Action activateRecovery = () => _recoverySuspend = false;
            activateRecovery.DelayInvoke(Settings.Core.Settings.Player.staminaRecoverDelay);
        }

        private IEnumerator Recovery()
        {
            while (true)
            {
                if (!_recoverySuspend)
                    Recover(_maxStamina * Settings.Core.Settings.Player.staminaRecoveryRate / 100);
                    
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Awake()
        {
            _indicator = GetComponent<StaminaIndicator>();
            GameManager.OnGameStarted += StartRecovery;
            GameManager.OnReplayGame += ReplayGame;

            _maxStamina = Settings.Core.Settings.Player.maxStamina;
        }

        private void ReplayGame()
        {
            StopAllCoroutines();
            _currentStamina = 100;

        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= StartRecovery;
            GameManager.OnReplayGame -= ReplayGame;
        }

        private void StartRecovery()
        {
            StartCoroutine(Recovery());
        }
    }
}