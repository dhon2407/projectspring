using Player;
using UnityEngine;

namespace Level.Destructibles
{
    public class GroundSpikes : MonoBehaviour, IEntity
    {
        public GameObject Owner => gameObject;
    }
}
