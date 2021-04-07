using System.Collections.Generic;
using CustomHelper;
using Level;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Level", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        public List<LevelSegmentHandler> segmentList;

        public float retryDelay = 2f;
        public float speedRateIncreasePerSegment;
        public float maxPlayerSpeed;

        public LevelSegmentHandler GetNextSegment()
        {
            return segmentList.GetRandom();
        }
    }
}