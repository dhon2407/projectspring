using CustomHelper;
using Level.Obstacles;
using UnityEngine;

namespace Player
{
    public sealed class HitBox : MonoBehaviour
    {
        public delegate void HitEvent(BaseObstacle obstacle);
        public event HitEvent OnHitObstacle;
        private void OnCollisionEnter2D(Collision2D other)
        {
            var target = other.gameObject.GetComponent<BaseObstacle>();
            if (target)
                OnHitObstacleInvoke(target);
        }

        private void OnHitObstacleInvoke(BaseObstacle obstacle)
        {
            OnHitObstacle?.Invoke(obstacle);
        }
    }
}