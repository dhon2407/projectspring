using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TutorialSettings", menuName = "Settings/Tutorial", order = 0)]
    public class TutorialSettings : ScriptableObject
    {
        [Title("Timing")]
        public float startLockActionDelay = 0.05f;
        public float onBlockSuccessDelay = 0.3f;
        public float blockingReducedTimeScale = 0.01f;
        public float attackingReducedTimeScale = 0.05f;
        public float jumpingReducedTimeScale = 0.05f;

        [Title("Messages")]
        public string blockingMessage = "SWIPE LEFT TO BLOCK!";
        public string attackingMessage = "TAP TO ATTACK!";
        public string jumpingMessage = "SWIPE UP TO JUMP!";
    }
}