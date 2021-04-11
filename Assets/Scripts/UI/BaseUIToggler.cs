using UnityEngine;

namespace UI
{
    public abstract class BaseUIToggler : MonoBehaviour
    {
        public bool IsActive { get; protected set; }
        public abstract bool Toggle();
        public abstract void SetTo(bool value);
    }
}