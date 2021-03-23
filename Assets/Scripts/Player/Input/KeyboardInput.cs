using Player.Input.Action;
using UnityEngine;

namespace Player.Input
{
    public class KeyboardInput : BaseInputHandler
    {
        [SerializeField]
        private bool pauseMovement;
        public override int MovementDirection => pauseMovement ? 0 : 1;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                InputActions.Enqueue(new JumpAction());
        }
    }
}