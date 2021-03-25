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
        private float maxStamina = 100;

        private float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                if (_currentStamina > value)
                    SuspendRecovery();
                
                _currentStamina = value;
                if (_indicator != null)
                    _indicator.Value = _currentStamina / maxStamina;
            }
        }

        private float _currentStamina = 100;
        private StaminaIndicator _indicator;
        private bool _recoverySuspend;

        public void Recover(float value)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina + value, 0, maxStamina);
        }

        public bool Consume(int needed)
        {
            if (HasStamina(needed))
            {
                CurrentStamina = Mathf.Clamp(CurrentStamina - needed, 0, maxStamina);
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
            activateRecovery.DelayInvoke(0.5f);
        }

        private IEnumerator Recovery()
        {
            while (true)
            {
                if (!_recoverySuspend) 
                    Recover(maxStamina * 0.04f);
                    
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Awake()
        {
            _indicator = GetComponent<StaminaIndicator>();
            GameManager.OnGameStarted += StartRecovery;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= StartRecovery;
        }

        private void StartRecovery()
        {
            StartCoroutine(Recovery());
        }
    }
}