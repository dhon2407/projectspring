using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [HideMonoScript]
    public class GroundSensor : MonoBehaviour
    {
        private int _triggeredColliderCount;
        private bool _tempDisabled;
        public bool OnGround => !_tempDisabled && _triggeredColliderCount > 0;

        public void TemporaryDisable(bool value)
        {
            _tempDisabled = value;
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