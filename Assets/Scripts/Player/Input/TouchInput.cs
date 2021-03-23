using CustomHelper;
using Lean.Touch;
using Player.Input.Action;
using UnityEngine;

namespace Player.Input
{
	public class TouchInput : BaseInputHandler
	{
		[SerializeField]
		private float requiredAngle;
		[SerializeField]
		private float requiredArc = -1.0f;
		[SerializeField]
		private float minSwipeValueDetection = 1;

		public override int MovementDirection => 1;
		
		private LeanScreenDepth _screenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

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

		private void HandleSwipe(LeanFinger finger)
		{
			HandleFingerSwipe(finger.StartScreenPosition, finger.ScreenPosition);
		}

		private void HandleFingerSwipe(Vector2 screenFrom, Vector2 screenTo)
		{
			if (!AngleIsValid(screenTo - screenFrom)) return;
			
			var worldFrom = _screenDepth.Convert(screenFrom, gameObject);
			var worldTo = _screenDepth.Convert(screenTo, gameObject);
			var verticalValue = (worldTo - worldFrom).y;

			if (Mathf.Sign(verticalValue) > 0 && Mathf.Abs(verticalValue) > minSwipeValueDetection)
				InputActions.Enqueue(new JumpAction());
		}

		private bool AngleIsValid(Vector2 vector)
		{
			if (!(requiredArc >= 0.0f)) return true;
			
			var angle = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
			var angleDelta = Mathf.DeltaAngle(angle, requiredAngle);

			return !(angleDelta < requiredArc * -0.5f) && !(angleDelta >= requiredArc * 0.5f);
		}

		private void HandleFingerTap(LeanFinger finger)
		{
			//ATTACK
		}
	}
}