using System;
using Managers;
using MEC;
using Player.Input.Action;
using UnityEngine;
using Utilities.Helpers;

namespace Player.Input
{
    public class KeyboardInput : BaseInputHandler
    {
        [SerializeField]
        private bool pauseMovement;

        private bool _stopMovement;
        private bool _tempDisabled;
        private bool _gameStarted;

        private const string ResumeMovementTag = "Resuming Movement";
        public override int MovementDirection => _stopMovement ? 0 : 1;
        public override void TemporaryStopMovement(float? duration)
        {
            _stopMovement = true;
            if (!duration.HasValue) return;
			
            System.Action resume = ()=> _stopMovement = false;
            resume.DelayInvoke(duration.Value);
        }

        public override void Suspend(bool includingMovement, float? duration = null)
        {
            if (includingMovement)
                _stopMovement = true;
			
            _tempDisabled = true;
            if (!duration.HasValue) return;
			
            System.Action resume = ()=> CancelSuspend(includingMovement);
            resume.DelayInvoke(duration.Value);
        }

        public override void CancelSuspend(bool includingMovement)
        {
            if (includingMovement)
                _stopMovement = false;
			
            _tempDisabled = false;
        }

        public override void ResumeMovement()
        {
            _stopMovement = false;
        }

        public override void BlockActions(Type actionType)
        {
            if (!BlockedActions.Contains(actionType))
                BlockedActions.Add(actionType);
        }

        public override void UnblockActions(Type actionType)
        {
            if (BlockedActions.Contains(actionType))
                BlockedActions.Remove(actionType);
        }
        
        private void Awake()
        {
            GameManager.OnGameStarted += GameStart;
            GameManager.OnReplayGame += ReplayGame;
        }

        private void ReplayGame()
        {
            CancelSuspend(true);
            _gameStarted = false;
            _tempDisabled = false;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= GameStart;
            GameManager.OnReplayGame -= ReplayGame;
        }
        
        private void GameStart()
        {
            _tempDisabled = false;
            System.Action enable = () => _gameStarted = true;
            enable.DelayInvoke(1f);
        }

        private void Update()
        {
            if (!_gameStarted || _tempDisabled)
                return;
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                EnqueueAction(new JumpAction());
            if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
                EnqueueAction(new JumpAction());
            if (UnityEngine.Input.GetKeyDown(KeyCode.X))
                EnqueueAction(new BasicAttack());
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
                EnqueueAction(new Block());
        }
        
        private void EnqueueAction(IAction action)
        {
            var actionType = action.GetType();
            if (BlockedActions.Contains(actionType))
                return;
			
            InputActions.Enqueue(action);
        }
    }
}