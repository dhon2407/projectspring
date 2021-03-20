using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [BoxGroup("Movement")]
        [MinValue(0)]
        public float baseMoveSpeed = 5f;
    }
}