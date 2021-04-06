using Player.Sensors;
using UnityEngine;

namespace Player.Enemy
{
    public class Bandit : BaseEnemy
    {
        [SerializeField]
        private PlayerSensor playerRangeSensor;
        
        [SerializeField]
        private PlayerSensor playerAttackSensor;

        private HurtBox _hurtBox;
        private static readonly int AnimState = Animator.StringToHash("AnimState");
        private bool _isAttacking;
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
            Animator.SetInteger(AnimState, 0);
        }

        private void OnPlayerOnRange(PlayerController player)
        {
            if (_isAttacking)
                return;
            
            _isAttacking = true;
            Animator.SetTrigger(AnimParamAttack);
        }

        private void OnSeePlayer(PlayerController player)
        {
            Animator.SetInteger(AnimState, AnimStateCombat);
        }

        private void SetupHurtBox()
        {
            _hurtBox = GetComponentInChildren<HurtBox>();
            _hurtBox.OnHit += OnHit;
        }

        private void OnHit(HitBox hitBox)
        {
            Animator.SetTrigger(Die);
            OnDie();
        }

        private void OnDie()
        {
            _hurtBox.OnHit -= OnHit;
            playerRangeSensor.OnPlayerDetect -= OnSeePlayer;
        }
    }
}