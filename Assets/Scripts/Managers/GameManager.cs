using System;
using CustomHelper;
using Lean.Touch;
using Managers.Core;
using Player;
using UnityEngine;
using Utilities.Helpers;

namespace Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
        #region EVENTS

        public delegate void BasicEvent();
        public delegate void DistanceChangeEvent(int value);
        public delegate void TimedEvent(float value);

        public static event BasicEvent OnGameStarted;
        public static event BasicEvent OnGameEnded;
        public static event BasicEvent OnReplayGame;
        public static event TimedEvent OnReplayGameReadyIn;
        public static event DistanceChangeEvent OnDistanceUpdate;

        #endregion
        
        public static float DistancePersonalRecord { get; set; }
        public static Vector3 PlayerSpawnPosition { get; private set; }
        public static string Username { get; private set; } = null;

        public static float DistanceTraveled
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
        
        public static void GameEnd()
        {
            OnGameEnded?.Invoke();
        }

        public static void SetUsername(string uName)
        {
            Username = uName;
        }

        public static float UpdateRecord()
        {
            DistancePersonalRecord =
                DistanceTraveled > DistancePersonalRecord ? DistanceTraveled : DistancePersonalRecord;
            
            LeaderBoardManager.SaveScore(_currentPlayer.playerName, (int)DistancePersonalRecord);

            return DistancePersonalRecord;
        }

        public static void PlayAgain()
        {
            var delayStart = 2f;

            Instance.Log($"Replaying game in {delayStart} second{(delayStart > 2 ? "s" : "")}.");
            _currentPlayer.transform.position = PlayerSpawnPosition;
            
            OnReplayGame?.Invoke();
            OnReplayGameReadyIn?.Invoke(delayStart);

            Action resume = Instance.Init;
            resume.DelayInvoke(delayStart);
        }
        
        public static void SetPlayer(PlayerController player)
        {
            _currentPlayer = player;
            _currentPlayer.playerName = Username;
            _startingPosition = _currentPlayer.transform.position;
            PlayerSpawnPosition = _startingPosition;
        }
        
        protected override void Init()
        {
            LeanTouch.OnFingerTap += HandleFingerTap;
            _gameStarted = false;
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
            if (!_gameStarted && Username != null)
            {
                this.Log("Starting game..");
                LeanTouch.OnFingerTap -= HandleFingerTap;
                OnGameStartedInvoke();
            }
        }
        
        private static void OnGameStartedInvoke()
        {
            DistanceTraveled = 0;
            Instance._gameStarted = true;
            OnGameStarted?.Invoke();
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