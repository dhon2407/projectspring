using UnityEngine;

namespace Level.Destructibles
{
    public class SimpleFX : MonoBehaviour
    {
        public delegate void EndEvent();
        public event EndEvent OnEnd;

        public void FXEnd()
        {
            OnEnd?.Invoke();
        }
    }
}
