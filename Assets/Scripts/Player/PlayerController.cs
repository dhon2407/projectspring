using Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [HideMonoScript]
    public class PlayerController : BaseCharacterController
    {
        public override void Jump()
        {
            //if (!Grounded)
            //    return;
            
            Animator.SetTrigger(AnimParamJump);
            Grounded = false;
            Animator.SetBool(AnimParamGrounded, Grounded);
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Settings.Core.Settings.Player.baseJumpForce);
        }
    }
}