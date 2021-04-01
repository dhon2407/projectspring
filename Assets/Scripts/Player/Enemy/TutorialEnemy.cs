using System.Collections;
using Audio;
using Level.Obstacles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Enemy
{
    public class TutorialEnemy : MonoBehaviour
    {
        public Vector2 throwOffForce;
        
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private static readonly int AnimParamAttack = Animator.StringToHash("Attack");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Die = Animator.StringToHash("Death");

        [Button]
        public void Attack()
        {
            _animator.SetTrigger(AnimParamAttack);
            StartCoroutine(SwordSound());
        }

        private IEnumerator SwordSound()
        {
            yield return new WaitForSeconds(0.1f);
            Sounds.Event.OnSwordSwoosh.Invoke();
        }

        [Button]
        public void ThrowOff()
        {
            _animator.SetTrigger(Jump);
            _rigidbody.AddForce(throwOffForce);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var target = other.gameObject.GetComponent<BaseObstacle>();
            if (target || target is GroundSpike)
            {
                _rigidbody.velocity = Vector2.zero;
                _animator.SetTrigger(Die);
            }
        }
    }
}
