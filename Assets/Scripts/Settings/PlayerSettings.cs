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
        [BoxGroup("Movement")]
        [MinValue(0)]
        public float baseJumpForce = 7.5f;
        [BoxGroup("Movement")]
        public bool disableMultipleJumps;

        [BoxGroup("Attributes")]
        public float staminaRecoverDelay = 0.5f;
        [BoxGroup("Attributes")]
        public float staminaRecoveryRate = 40f;
        [BoxGroup("Attributes")]
        public float maxStamina = 100;
    }
}