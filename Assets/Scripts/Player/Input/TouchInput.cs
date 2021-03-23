using Lean.Touch;
using Player.Input.Action;

namespace Player.Input
{
    public class TouchInput : BaseInputHandler
    {
        public override int MovementDirection => 0;
        
        private void OnEnable()
        {
            LeanTouch.OnFingerTap += HandleFingerTap;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerTap -= HandleFingerTap;
        }
        
        private void HandleFingerTap(LeanFinger finger)
        {
            InputActions.Enqueue(new JumpAction());
        }
    }
}