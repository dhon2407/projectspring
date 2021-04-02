using Audio;
using Level.Obstacles;
using Managers;
using Player.Input;
using Player.Input.Action;
using Player.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public abstract class BasePlayerController : MonoBehaviour, IPlayerController, IEntity
    {
        #region Inspector Values

        [ShowInInspector, ShowIf("@InputHandler != null"), LabelText("Input:")]
        private IInputHandler CurrentInputHandler => InputHandler;
        
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
        protected StaminaHandler Stamina;
        protected HurtBox HurtBox;

        protected bool Grounded;
        protected bool Moving;
        protected int FacingDirection;
        private IPlayerState _currentState;
        private bool _gameStarted;
        
        public bool OnGround
        {
            get => Grounded && GroundSensor.OnGround;
            set => Grounded = value;
        }

        protected static readonly IPlayerState DefaultState = new FreeState();

        public IPlayerState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState.Exit();
                _currentState = value;
            } 
        }

        public IAction CurrentAction { get; set; } = null;

        public int MoveDirection => _gameStarted ? InputHandler?.MovementDirection ?? 0 : 0;
        
        public abstract void Jump();
        public abstract void DoAction(IAction action);
        public abstract void HandleCollision(HitBox hitBox);
        
        public bool HasStamina(int requiredStamina)
        {
            return Stamina.HasStamina(requiredStamina);
        }
        
        public bool ConsumeStamina(int requiredStamina)
        {
            return Stamina.Consume(requiredStamina);
        }

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
            InitializeComponents();
            _currentState = DefaultState;

            GameManager.OnGameStarted += GameStart;
            GameManager.OnReplayGame += ResetGame;
        }

        private void InitializeComponents()
        {
            Animator = GetComponent<Animator>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            GroundSensor = FindObjectOfType<GroundSensor>();
            Renderer = GetComponent<SpriteRenderer>();
            InputHandler = GetComponent<IInputHandler>();
            Stamina = GetComponentInChildren<StaminaHandler>();
            
            HurtBox = GetComponentInChildren<HurtBox>();
            if (HurtBox)
                HurtBox.OnHit += HandleCollision;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= GameStart;
            GameManager.OnReplayGame -= ResetGame;
            
            if (HurtBox)
                HurtBox.OnHit -= HandleCollision;
        }

        protected virtual void ResetGame()
        {
            _gameStarted = false;
            Animator.Rebind();
            Animator.Update(0f);
        }

        protected virtual void GameStart()
        {
            _gameStarted = true;
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
            {
                OnGroundLandInvoke();
                Grounded = true;
            }

            if (Grounded && !GroundSensor.OnGround)
                Grounded = false;
            
            Animator.SetBool(AnimParamGrounded, Grounded);
        }

        public GameObject Owner => gameObject;
    }
}