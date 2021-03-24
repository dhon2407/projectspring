using System;
using Lean.Touch;
using Managers.Core;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
        #region EVENTS

        public delegate void BasicEvent();
        public delegate void DistanceChangeEvent(int value);

        public static event BasicEvent OnGameStarted;
        public static event BasicEvent OnGameEnded;
        public static event DistanceChangeEvent OnDistanceUpdate;

        #endregion

        private static float DistanceTraveled
        {
            get => _distanceTraveled;
            set
            {
                if (Math.Abs(value - _distanceTraveled) < 0.5f)
                    OnDistanceUpdateInvoke((int)value);

                _distanceTraveled = value;
            }
        }
        private static Vector3 _startingPosition;
        private static PlayerController _currentPlayer;
        private static float _distanceTraveled;

        private bool _gameStarted;
        
        public static void SetPlayer(PlayerController player)
        {
            _currentPlayer = player;
            _startingPosition = _currentPlayer.transform.position;
        }
        
        protected override void Init()
        {
            LeanTouch.OnFingerTap += HandleFingerTap;
        }

        private void LateUpdate()
        {
            if (!_gameStarted)
                return;
            
            UpdateDistanceTraveled();
        }

        private void UpdateDistanceTraveled()
        {
            DistanceTraveled = _currentPlayer.transform.position.x - _startingPosition.x;
        }

        private void HandleFingerTap(LeanFinger finger)
        {
            if (!_gameStarted)
                OnGameStartedInvoke();
        }
        
        private static void OnGameStartedInvoke()
        {
            DistanceTraveled = 0;
            Instance._gameStarted = true;
            OnGameStarted?.Invoke();
        }

        private static void OnGameEndedInvoke()
        {
            OnGameEnded?.Invoke();
        }
        
        private static void OnDistanceUpdateInvoke(int value)
        {
            OnDistanceUpdate?.Invoke(value);
        }

        #region Singleton Attributes

        private static GameManager Instance =>
            _instance ? _instance : throw new UnityException($"No instance of {nameof(GameManager)}");
        
        private static GameManager _instance;

        protected override GameManager Self
        {
            set => _instance = value;
            get => _instance;
        }

        #endregion
    }
}