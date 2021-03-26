using System;
using Lean.Touch;
using Managers;
using Player.Input.Action;
using UnityEngine;
using Utilities.Helpers;

namespace Player.Input
{
	public class TouchInput : BaseInputHandler
	{
		[SerializeField]
		private float requiredAngle = 0;
		[SerializeField]
		private float requiredArc = -1.0f;
		[SerializeField]
		private float minSwipeValueDetection = 1;

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

		private LeanScreenDepth _screenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);
		private bool _stopMovement;
		private bool _gameStarted;
		private bool _tempDisabled;

		private void Awake()
		{
			GameManager.OnGameStarted += GameStart;
		}

		private void OnDestroy()
		{
			GameManager.OnGameStarted -= GameStart;
		}

		private void GameStart()
		{
			_tempDisabled = false;
			System.Action enable = () => _gameStarted = true;
			enable.DelayInvoke(1f);
		}
		
		private void OnEnable()
		{
			LeanTouch.OnFingerTap += HandleFingerTap;
			LeanTouch.OnFingerSwipe += HandleSwipe;
		}

		private void OnDisable()
		{
			LeanTouch.OnFingerTap -= HandleFingerTap;
			LeanTouch.OnFingerSwipe -= HandleSwipe;
		}
		
		private void HandleFingerTap(LeanFinger finger)
		{
			if (!_gameStarted || _tempDisabled)
				return;
			
			EnqueueAction(new BasicAttack());
		}

		private void HandleSwipe(LeanFinger finger)
		{
			if (!_gameStarted || _tempDisabled)
				return;
			
			HandleFingerSwipe(finger.StartScreenPosition, finger.ScreenPosition);
		}

		private void HandleFingerSwipe(Vector2 screenFrom, Vector2 screenTo)
		{
			if (!AngleIsValid(screenTo - screenFrom)) return;
			
			var worldFrom = _screenDepth.Convert(screenFrom, gameObject);
			var worldTo = _screenDepth.Convert(screenTo, gameObject);
			var verticalValue = (worldTo - worldFrom).y;
			var horizontalValue = (worldTo - worldFrom).x;

			if (Mathf.Sign(verticalValue) > 0 && Mathf.Abs(verticalValue) > minSwipeValueDetection)
				EnqueueAction(new JumpAction());
			else if (Mathf.Sign(horizontalValue) < 0 && Mathf.Abs(horizontalValue) > minSwipeValueDetection)
				EnqueueAction(new Block());
		}

		private void EnqueueAction(IAction action)
		{
			var actionType = action.GetType();
			if (BlockedActions.Contains(actionType))
				return;
			
			InputActions.Enqueue(action);
		}

		private bool AngleIsValid(Vector2 vector)
		{
			if (!(requiredArc >= 0.0f)) return true;
			
			var angle = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
			var angleDelta = Mathf.DeltaAngle(angle, requiredAngle);

			return !(angleDelta < requiredArc * -0.5f) && !(angleDelta >= requiredArc * 0.5f);
		}
	}
}