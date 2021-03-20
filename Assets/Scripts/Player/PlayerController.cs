using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [HideMonoScript]
    public class PlayerController : MonoBehaviour
    {
        #region Inspector Values

        

        #endregion

        #region Events

        public delegate void BasicEvent();

        public event BasicEvent OnGroundLand;

        #endregion

        #region Animator Parameters

        private static readonly int AnimParamAirSpeedY = Animator.StringToHash("AirSpeedY");
        private static readonly int AnimParamGrounded = Animator.StringToHash("Grounded");
        private static readonly int AnimMovementState = Animator.StringToHash("AnimState");

        #endregion

        public bool OnGround
        {
            get => _grounded && _groundSensor.OnGround;
            set => _grounded = value;
        }
        
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private GroundSensor _groundSensor;
        private SpriteRenderer _renderer;

        private bool _grounded;
        private bool _moving;
        private int _facingDirection;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _groundSensor = FindObjectOfType<GroundSensor>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            CheckGround();
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var moveDirection = 1;
            UpdateMovingStatus(moveDirection);
            UpdateFacingDirection(moveDirection);
            UpdateMoveVelocity(moveDirection);

            _animator.SetInteger(AnimMovementState, _moving ? 1 : 0);
        }

        private void UpdateMoveVelocity(int moveDirection)
        {
            var verticalVelocity = _rigidbody2D.velocity.y;
            _rigidbody2D.velocity =
                new Vector2(moveDirection * Settings.Core.Settings.Player.baseMoveSpeed, verticalVelocity);
            _animator.SetFloat(AnimParamAirSpeedY, verticalVelocity);
        }

        private void UpdateMovingStatus(int moveDirection)
        {
            if (Mathf.Abs(moveDirection) > Mathf.Epsilon && (int) Mathf.Sign(moveDirection) == _facingDirection)
                _moving = true;
            else
                _moving = false;
        }

        private void UpdateFacingDirection(int moveDirection)
        {
            if (moveDirection == 0)
                return;

            _renderer.flipX = moveDirection < 0;
            _facingDirection = moveDirection < 0 ? -1 : 1;
        }

        private void CheckGround()
        {
            if (!_grounded && _groundSensor.OnGround)
                _grounded = true;

            if (_grounded && !_groundSensor.OnGround)
                _grounded = false;
            
            _animator.SetBool(AnimParamGrounded, _grounded);
        }
        
        private void OnGroundLandInvoke()
        {
            OnGroundLand?.Invoke();
        }
    }
}