using System;
using UnityEngine;

namespace Player.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IEntity
    {
        public GameObject Owner => gameObject;
        
        protected Animator Animator;
        protected Rigidbody2D Rigidbody;
        protected static readonly int AnimParamAttack = Animator.StringToHash("Attack");
        protected static readonly int Jump = Animator.StringToHash("Jump");
        protected static readonly int Die = Animator.StringToHash("Death");

        protected virtual void Init()
        {
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            Init();
        }
    }
}