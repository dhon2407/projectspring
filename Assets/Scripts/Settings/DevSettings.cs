using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Developer Settings", menuName = "Settings/Dev", order = 0)]
    public class DevSettings : ScriptableObject
    {
        [BoxGroup("Level")]
        public float levelSegmentDestroyDelay = 0.5f;
        [BoxGroup("Leader Board")]
        public int leaderBoardsLimit = 15;
    }
}