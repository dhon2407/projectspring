namespace Player.Enemy
{
    public class Bandit : BaseEnemy
    {
        private HurtBox _hurtBox;
        protected override void Init()
        {
            base.Init();
            SetupHurtBox();
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
        }
    }
}