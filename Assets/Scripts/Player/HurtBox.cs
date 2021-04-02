using System;
using UnityEngine;

namespace Player
{
    public sealed class HurtBox : MonoBehaviour
    {
        public IEntity Owner => _owner;
        private IEntity _owner;
        
        public delegate void HitEvent(HitBox hitBox);
        public event HitEvent OnHit;

        private void Awake()
        {
            _owner = GetComponentInParent<IEntity>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            TriggerEnter(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnter(other.gameObject);
        }
        
        private void TriggerEnter(GameObject other)
        {
            var target = other.GetComponent<HitBox>();
            if (target && target.Owner != Owner)
                OnHitObstacleInvoke(target);
        }

        private void OnHitObstacleInvoke(HitBox obstacle)
        {
            OnHit?.Invoke(obstacle);
        }
    }
}