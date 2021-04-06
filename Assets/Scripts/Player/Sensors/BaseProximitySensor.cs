using System;
using UnityEngine;
using Utilities.Helpers;

namespace Player.Sensors
{
    public abstract class BaseProximitySensor : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask detectionMask = 0;

        public delegate void OnDetectEvent(Component type);
        public event OnDetectEvent OnDetect;
        public event OnDetectEvent OnLeave;

        private Type[] _typesToDetect;

        protected void SetTypeToDetect(Type[] types)
        {
            _typesToDetect = types;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEvent(other, OnDetect);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerEvent(other, OnLeave);
        }

        private void OnTriggerEvent(Collider2D other, OnDetectEvent actionEvent)
        {
            if (!detectionMask.Contains(other.gameObject.layer))
                return;

            foreach (var type in _typesToDetect)
            {
                var comp = other.GetComponent(type);
                if (comp != null)
                    actionEvent?.Invoke(comp);
            }
        }
    }
}