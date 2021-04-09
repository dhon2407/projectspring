using System.Collections;
using CustomHelper;
using Player.Sensors;
using UnityEngine;

namespace Player.Enemy
{
    public class RunningBandit  : BaseEnemy
    {
        [SerializeField]
        private PlayerSensor playerRangeSensor;
        
        [SerializeField]
        private PlayerSensor playerAttackSensor;

        private HurtBox _hurtBox;
        private static readonly int AnimState = Animator.StringToHash("AnimState");
        private bool _isAttacking;
        private bool _running;
        private Vector2 _velocity;
        private const int AnimStateCombat = 1;

        public void OnAttackFinish()
        {
            _isAttacking = false;
        }

        protected override void Init()
        {
            base.Init();
            SetupHurtBox();
            SetupSensors();
        }

        private void SetupSensors()
        {
            playerRangeSensor.OnPlayerDetect += OnSeePlayer;
            playerAttackSensor.OnPlayerDetect += OnPlayerOnRange;
            playerRangeSensor.OnPlayerLeave += OnPlayerLeave;
            playerAttackSensor.OnPlayerLeave += OnPlayerLeave;
        }

        private void OnPlayerLeave(PlayerController player)
        {
            _running = false;
            Rigidbody.velocity = Vector2.zero;
            Animator.SetInteger(AnimState, 0);
        }

        private void OnPlayerOnRange(PlayerController player)
        {
            if (_isAttacking || player.IsDead)
                return;
            
            _isAttacking = true;
            Animator.SetTrigger(AnimParamAttack);
        }

        private void OnSeePlayer(PlayerController player)
        {
            if (player.IsDead)
                return;

            StartRunning();
        }

        private void StartRunning()
        {
            _running = true;
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_running)
            {
                _velocity.x = _isAttacking ? 0 : -Settings.Core.Settings.Level.enemyMovementSpeed * Time.deltaTime;
                _velocity.y = Rigidbody.velocity.y;
                this.Log($"Moving velocity {_velocity}");
                Rigidbody.velocity = _velocity;
                Animator.SetInteger(AnimState, 2);
                yield return null;
            }
        }

        private void SetupHurtBox()
        {
            _hurtBox = GetComponentInChildren<HurtBox>();
            _hurtBox.OnHit += OnHit;
        }

        private void OnHit(HitBox hitBox)
        {
            if (IsDead)
                return;
            
            OnDie();
        }

        private void OnDie()
        {
            IsDead = true;
            _running = false;
            Rigidbody.velocity = Vector2.zero;
            Animator.SetTrigger(Die);
            _hurtBox.OnHit -= OnHit;
            playerRangeSensor.OnPlayerDetect -= OnSeePlayer;
            playerAttackSensor.OnPlayerDetect -= OnPlayerOnRange;
            playerRangeSensor.OnPlayerLeave -= OnPlayerLeave;
            playerAttackSensor.OnPlayerLeave -= OnPlayerLeave;
        }
    }
}