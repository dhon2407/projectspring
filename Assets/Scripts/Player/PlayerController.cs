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

        #endregion

        public bool OnGround
        {
            get => _grounded && _groundSensor.OnGround;
            set => _grounded = value;
        }
        
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private bool _grounded;
        private GroundSensor _groundSensor;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _groundSensor = FindObjectOfType<GroundSensor>();
        }

        private void Update()
        {
            CheckGround();
            _animator.SetFloat(AnimParamAirSpeedY, _rigidbody2D.velocity.y);
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