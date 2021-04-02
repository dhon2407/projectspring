using UnityEngine;

namespace Player
{
    public class HitBox : MonoBehaviour
    {
        public IEntity Owner { get => _owner; }

        private IEntity _owner;
        
        private void Awake()
        {
            _owner = GetComponentInParent<IEntity>();
        }
    }
}