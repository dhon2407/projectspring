using UnityEngine;

namespace Level
{
    public sealed class SegmentTrigger : MonoBehaviour
    {
        public delegate void BasicEvent();

        public event BasicEvent OnTrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerInvoke();
        }


        private void OnTriggerInvoke()
        {
            OnTrigger?.Invoke();
        }
    }
}