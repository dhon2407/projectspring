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

        private const string ResumeMovementTag = "Resuming Movement";
        public override int MovementDirection => pauseMovement ? 0 : 1;
        public override void TemporaryStopMovement(float? duration)
        {
            Timing.KillCoroutines(ResumeMovementTag);
            pauseMovement = true;
            if (!duration.HasValue) return;
			
            System.Action resume = ()=> pauseMovement = false;
            resume.DelayInvoke(duration.Value, ResumeMovementTag);
        }

        public override void Suspend(bool includingMovement, float? duration = null)
        {
            //TODO less priority
        }

        public override void CancelSuspend(bool includingMovement)
        {
            //TODO less priority
        }

        public override void ResumeMovement()
        {
            Timing.KillCoroutines(ResumeMovementTag);
            pauseMovement = false;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                InputActions.Enqueue(new JumpAction());
        }
    }
}