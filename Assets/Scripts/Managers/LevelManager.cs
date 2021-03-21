using Level;
using Managers.Core;
using UnityEngine;

namespace Managers
{
    public class LevelManager : SingletonManager<LevelManager>
    {
        public static void RegisterSegment(LevelSegmentHandler levelSegmentHandler)
        {
            levelSegmentHandler.OnLevelSegmentEnding += SegmentEnding;
        }

        private static void SegmentEnding(LevelSegmentHandler segmentOwner, Transform endPoint)
        {
            segmentOwner.OnLevelSegmentEnding -= SegmentEnding;
            var nextSegment = Instantiate(Settings.Core.Settings.Level.GetNextSegment(), endPoint.position,
                Quaternion.identity);
            
            nextSegment.OnLevelSegmentStarting += owner =>
            {
                segmentOwner.DestroySelf();
            };
        }

        protected override void Init()
        {
            
        }
        
        private static LevelManager Instance =>
            _instance ? _instance : throw new UnityException($"No instance of {nameof(LevelManager)}");
        
        private static LevelManager _instance;

        protected override LevelManager Self
        {
            set => _instance = value;
            get => _instance;
        }
    }
}