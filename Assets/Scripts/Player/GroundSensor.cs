using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Helpers;

namespace Player
{
    [HideMonoScript]
    public class GroundSensor : MonoBehaviour
    {
        private int _triggeredColliderCount;
        private bool _tempDisabled;
        public bool OnGround => !_tempDisabled && _triggeredColliderCount > 0;

        public void TemporaryDisable(float? time = null)
        {
            _tempDisabled = true;
            if (!time.HasValue) return;
            
            Action enable = Enable;
            enable.DelayInvoke(time.Value);
        }

        public void Enable()
        {
            _tempDisabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _triggeredColliderCount++;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _triggeredColliderCount = Mathf.Clamp(_triggeredColliderCount - 1, 0, int.MaxValue);
        }
    }
}