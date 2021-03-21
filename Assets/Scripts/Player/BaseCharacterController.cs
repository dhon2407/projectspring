﻿using Audio;
using Player.Input;
using UnityEngine;

namespace Player
{
    public abstract class BaseCharacterController : MonoBehaviour, IPlayerController
    {
        #region Inspector Values

        #endregion

        #region Events

        public event BasicEvent OnGroundLand;
        public event BasicEvent OnJump;

        #endregion

        #region Animator Parameters

        protected static readonly int AnimParamAirSpeedY = Animator.StringToHash("AirSpeedY");
        protected static readonly int AnimParamGrounded = Animator.StringToHash("Grounded");
        protected static readonly int AnimParamMovementState = Animator.StringToHash("AnimState");
        protected static readonly int AnimParamJump = Animator.StringToHash("Jump");

        #endregion

        protected Animator Animator;
        protected Rigidbody2D Rigidbody2D;
        protected GroundSensor GroundSensor;
        protected SpriteRenderer Renderer;
        protected IInputHandler InputHandler;

        protected bool Grounded;
        protected bool Moving;
        protected int FacingDirection;
        
        public bool OnGround
        {
            get => Grounded && GroundSensor.OnGround;
            set => Grounded = value;
        }

        public int MoveDirection => InputHandler?.MovementDirection ?? 0;

        public abstract void Jump();
        
        public virtual void OnJumpInvoke()
        {
            OnJump?.Invoke();
            Sounds.Event.OnPlayerJump.Invoke();
        }
        
        public virtual void OnGroundLandInvoke()
        {
            Sounds.Event.OnPlayerLand.Invoke();
            OnGroundLand?.Invoke();
        }

        public virtual void OnRunStep()
        {
            Sounds.Event.OnFootStep.Invoke();
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            GroundSensor = FindObjectOfType<GroundSensor>();
            Renderer = GetComponent<SpriteRenderer>();
            InputHandler = GetComponent<IInputHandler>();
        }

        private void Update()
        {
            CheckGround();
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            UpdateMovingStatus();
            UpdateFacingDirection();
            UpdateMoveVelocity();

            CheckInput();
            
            Animator.SetInteger(AnimParamMovementState, Moving ? 1 : 0);
        }

        private void CheckInput()
        {
            if (InputHandler == null)
                return;
            
            var inputAction = InputHandler.ConsumeCurrentInputAction;
            inputAction?.Execute(this);
        }

        private void UpdateMoveVelocity()
        {
            var verticalVelocity = Rigidbody2D.velocity.y;
            Rigidbody2D.velocity =
                new Vector2(MoveDirection * Settings.Core.Settings.Player.baseMoveSpeed, verticalVelocity);
            Animator.SetFloat(AnimParamAirSpeedY, verticalVelocity);
        }

        private void UpdateMovingStatus()
        {
            if (Mathf.Abs(MoveDirection) > Mathf.Epsilon && (int) Mathf.Sign(MoveDirection) == FacingDirection)
                Moving = true;
            else
                Moving = false;
        }

        private void UpdateFacingDirection()
        {
            if (MoveDirection == 0)
                return;

            Renderer.flipX = MoveDirection < 0;
            FacingDirection = MoveDirection < 0 ? -1 : 1;
        }

        private void CheckGround()
        {
            if (!Grounded && GroundSensor.OnGround)
                Grounded = true;

            if (Grounded && !GroundSensor.OnGround)
                Grounded = false;
            
            Animator.SetBool(AnimParamGrounded, Grounded);
        }
    }
}