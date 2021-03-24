using System.Collections.Generic;
using Player.Input.Action;
using Sirenix.OdinInspector;
using UnityEngine;
using GameSettings = Settings.Core.Settings;

namespace Player
{
    [HideMonoScript]
    public class PlayerController : BaseCharacterController
    {
        //TODO Adjustable somewhere
        [Range(1,3)]
        public int maxAttackUnlock = 1;

        private static readonly int AnimParamBasicAttack1 = Animator.StringToHash("Attack1");
        private static readonly int AnimParamBasicAttack2 = Animator.StringToHash("Attack2");
        private static readonly int AnimParamBasicAttack3 = Animator.StringToHash("Attack3");

        private int _attackSequenceNo = 0;
        private bool _onAttack;

        private readonly Queue<int> _attackParamSequence = new Queue<int>();
        
        public void OnAttackComplete()
        {
            if (_attackParamSequence.Count > 0)
            {
                Animator.SetTrigger( _attackParamSequence.Dequeue());
                return;
            }
            
            _attackSequenceNo = 0;
            _onAttack = false;
            InputHandler.ResumeMovement();
        }

        public override void Jump()
        {
            if (JumpRestrictions())
                return;
            
            Animator.SetTrigger(AnimParamJump);
            Grounded = false;
            Animator.SetBool(AnimParamGrounded, Grounded);
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Settings.Core.Settings.Player.baseJumpForce);
            GroundSensor.TemporaryDisable(0.2f);
        }

        public override void DoAction(IAction action)
        {
            if (action is BasicAttack)
                HandleAttackAction();
        }

        private void HandleAttackAction()
        {
            if (!_onAttack)
                StartInitialAttack();
            else
                HandleChainingAttack();
        }

        private void HandleChainingAttack()
        {
            if (_attackSequenceNo >= maxAttackUnlock)
                return;

            _attackParamSequence.Enqueue(GetAttackParam(_attackSequenceNo++));
        }

        private void StartInitialAttack()
        {
            _onAttack = true;
            Animator.SetTrigger(GetAttackParam(_attackSequenceNo++));
            InputHandler.TemporaryStopMovement();
        }

        private int GetAttackParam(int attackSequenceNo)
        {
            return attackSequenceNo switch
            {
                0 => AnimParamBasicAttack1,
                1 => AnimParamBasicAttack2,
                2 => AnimParamBasicAttack3,
                _ => throw new UnityException("No such attack animation trigger")
            };
        }

        private bool JumpRestrictions()
        {
            return (!Grounded &&
                   GameSettings.Player.disableMultipleJumps) ||
                   _onAttack;
        }
    }
}