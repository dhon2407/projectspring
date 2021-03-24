using System;
using System.Collections.Generic;
using Player.Input.Action;
using Player.States;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Helpers;
using GameSettings = Settings.Core.Settings;

namespace Player
{
    [HideMonoScript]
    public class PlayerController : BaseCharacterController
    {
        private readonly Queue<int> _attackParamSequence = new Queue<int>();

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
        
        private bool JumpRestrictions()
        {
            return !Grounded &&
                   GameSettings.Player.disableMultipleJumps ||
                   !CurrentState.CanJump;
        }

        public override void DoAction(IAction action)
        {
            if (action is BasicAttack)
                HandleAttackAction();
            else if (action is Block)
                HandleBlock();
        }

        //TODO Move to other class / refactoring
        #region BLOCK HANDLING
        
        //TODO Adjustable somewhere
        public float blockDuration = 3f;

        private static readonly int AnimParamBlock = Animator.StringToHash("Block");
        private static readonly int AnimParamBlockLock = Animator.StringToHash("IdleBlock");
        
        private void HandleBlock()
        {
            if (!CurrentState.CanMoveTo<BlockState>()) return; 
            
            CurrentState = new BlockState();
            
            Animator.SetBool(AnimParamBlockLock, true);
            Animator.SetTrigger(AnimParamBlock);
            InputHandler.TemporaryStopMovement();

            Action releaseBlock = OnBlockComplete;
            releaseBlock.DelayInvoke(blockDuration);
        }

        private void OnBlockComplete()
        {
            Animator.SetBool(AnimParamBlockLock, false);
            InputHandler.ResumeMovement();
            CurrentState = DefaultState;
        }

        #endregion

        //TODO Move to other class / refactoring
        #region ATTACK HANDLING
        
        //TODO Adjustable somewhere
        [Range(1,3)]
        public int maxAttackUnlock = 1;

        private static readonly int AnimParamBasicAttack1 = Animator.StringToHash("Attack1");
        private static readonly int AnimParamBasicAttack2 = Animator.StringToHash("Attack2");
        private static readonly int AnimParamBasicAttack3 = Animator.StringToHash("Attack3");

        private int _attackSequenceNo = 0;
        private bool _onAttack;

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
            CurrentState = DefaultState;
        }

        private void HandleAttackAction()
        {
            if (!CurrentState.CanMoveTo<AttackState>())
                return;

            if (!CurrentState.Is<AttackState>())
                InitializeAttackState();

            if (!_onAttack)
                StartInitialAttack();
            else
                HandleChainingAttack();
        }

        private void InitializeAttackState()
        {
            CurrentState = new AttackState();
            CurrentState.OnExit += () =>
            {
                _attackSequenceNo = 0;
                _onAttack = false;
            };
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

        #endregion
    }
}