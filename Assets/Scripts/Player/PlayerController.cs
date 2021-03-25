﻿using System;
using System.Collections.Generic;
using Audio;
using Level.Obstacles;
using Managers;
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
        //TODO Move to other class / refactoring
        #region TUTORIAL HANDLING
        public void LockAllActions()
        {
            InputHandler.BlockActions.Add(typeof(JumpAction));
            InputHandler.BlockActions.Add(typeof(BasicAttack));
            InputHandler.BlockActions.Add(typeof(Block));
        }
        public void UnlockJump()
        {
            if (InputHandler.BlockActions.Contains(typeof(JumpAction)))
                InputHandler.BlockActions.Remove(typeof(JumpAction));
        }

        public void UnlockAttack()
        {
            if (InputHandler.BlockActions.Contains(typeof(BasicAttack)))
                InputHandler.BlockActions.Remove(typeof(BasicAttack));
        }

        #endregion
        
        
        //TODO Move to other class / refactoring
        #region JUMP HANDLING

        public event BasicEvent OnStartJump;
        public override void Jump()
        {
            if (JumpRestrictions() || !Stamina.Consume(CurrentAction.RequiredStamina))
                return;

            OnStartJump?.Invoke();
            Animator.SetTrigger(AnimParamJump);
            Grounded = false;
            Animator.SetBool(AnimParamGrounded, Grounded);
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, GameSettings.Player.baseJumpForce);
            GroundSensor.TemporaryDisable(0.2f);
        }
        
        private bool JumpRestrictions()
        {
            return !Grounded &&
                   GameSettings.Player.disableMultipleJumps ||
                   !CurrentState.CanJump;
        }

        #endregion

        public override void DoAction(IAction action)
        {
            CurrentAction = action;
            if (action is BasicAttack)
                HandleAttackAction();
            else if (action is Block)
                HandleBlock();
        }

        public override void HandleObstacleCollision(BaseObstacle obstacle)
        {
            if (obstacle is GroundSpike)
                Die();
        }

        private void Die()
        {
            Animator.SetBool(NoBlood,true);
            Animator.SetTrigger(Death);
            InputHandler.Suspend(true);
        }

        protected override void GameStart()
        {
            base.GameStart();
            GameManager.SetPlayer(this);
        }

        //TODO Move to other class / refactoring
        #region BLOCK HANDLING
        
        //TODO Adjustable somewhere
        public float blockDuration = 3f;

        private static readonly int AnimParamBlock = Animator.StringToHash("Block");
        private static readonly int AnimParamBlockLock = Animator.StringToHash("IdleBlock");
        
        private void HandleBlock()
        {
            if (!CurrentState.CanMoveTo<BlockState>())
                return; 
            
            if (!Stamina.Consume(CurrentAction.RequiredStamina))
                return;
            
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
            CurrentAction = null;
        }

        #endregion

        //TODO Move to other class / refactoring
        #region ATTACK HANDLING

        public event BasicEvent OnStartAttack;
        
        //TODO Adjustable somewhere
        [Range(1,3)]
        public int maxAttackUnlock = 1;

        private readonly Queue<int> _attackParamSequence = new Queue<int>();
        
        private static readonly int AnimParamBasicAttack1 = Animator.StringToHash("Attack1");
        private static readonly int AnimParamBasicAttack2 = Animator.StringToHash("Attack2");
        private static readonly int AnimParamBasicAttack3 = Animator.StringToHash("Attack3");
        private static readonly int Death = Animator.StringToHash("Death");

        private int _attackSequenceNo = 0;
        private bool _onAttack;
        private static readonly int NoBlood = Animator.StringToHash("noBlood");

        public void OnAttackComplete()
        {
            if (_attackParamSequence.Count > 0)
            {
                if (!Stamina.Consume(CurrentAction.RequiredStamina))
                {
                    InputHandler.ResumeMovement();
                    return;
                }

                OnStartAttack?.Invoke();
                Sounds.Event.OnSwordSwoosh.Invoke();
                Animator.SetTrigger( _attackParamSequence.Dequeue());
                return;
            }
            
            _attackSequenceNo = 0;
            _onAttack = false;
            InputHandler.ResumeMovement();
            CurrentState = DefaultState;
            CurrentAction = null;
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
            if (!Stamina.Consume(CurrentAction.RequiredStamina))
                return;
            
            _onAttack = true;
            OnStartAttack?.Invoke();
            Sounds.Event.OnSwordSwoosh.Invoke();
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