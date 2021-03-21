using System;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Helpers;

namespace Level
{
    public class LevelSegmentHandler : MonoBehaviour
    {
        [Required, SerializeField]
        private Transform endPoint;
        [Required, SerializeField]
        private SegmentTrigger nearEndTrigger;
        [Required, SerializeField]
        private SegmentTrigger startingTrigger;

        public delegate void LevelSegmentEndingEvent(LevelSegmentHandler segmentOwner, Transform endPoint);
        public delegate void LevelSegmentStaringEvent(LevelSegmentHandler segmentOwner);

        public event LevelSegmentEndingEvent OnLevelSegmentEnding;
        public event LevelSegmentStaringEvent OnLevelSegmentStarting;

        private void Awake()
        {
            if (nearEndTrigger is { })
                nearEndTrigger.OnTrigger += NearEndTriggered;
            
            if (startingTrigger is { })
                startingTrigger.OnTrigger += StaringTriggered;
        }

        private void Start()
        {
            LevelManager.RegisterSegment(this);
        }

        private void NearEndTriggered()
        {
            OnLevelEndingInvoke(endPoint);
        }
        
        private void StaringTriggered()
        {
            OnLevelSegmentStartingInvoke();
        }

        protected virtual void OnLevelEndingInvoke(Transform endTransform)
        {
            OnLevelSegmentEnding?.Invoke(this, endTransform);
        }
        
        protected virtual void OnLevelSegmentStartingInvoke()
        {
            OnLevelSegmentStarting?.Invoke(this);
        }

        public void DestroySelf()
        {
            Action destroySelf = () => Destroy(gameObject);
            destroySelf.DelayInvoke(0.5f);
        }
    }
}