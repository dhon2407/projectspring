using Level;
using Managers.Core;
using UnityEngine;

namespace Managers
{
    public class LevelManager : SingletonManager<LevelManager>
    {
        private static int _levelSegmentGenerated = 0;
        public static void RegisterSegment(LevelSegmentHandler levelSegmentHandler)
        {
            levelSegmentHandler.OnLevelSegmentEnding += SegmentEnding;
        }

        private static void SegmentEnding(LevelSegmentHandler segmentOwner, Transform endPoint)
        {
            _levelSegmentGenerated++;

            var nextSegmentRef = Settings.Core.Settings.Level.GetNextSegment();
            segmentOwner.OnLevelSegmentEnding -= SegmentEnding;
            
            var nextSegment = Instantiate(nextSegmentRef, endPoint.position,
                Quaternion.identity);
            nextSegment.name = $"{nextSegmentRef.name} - {_levelSegmentGenerated}";
            nextSegment.OnLevelSegmentStarting += owner =>
            {
                if (segmentOwner != null)
                    segmentOwner.DestroySelf();
            };
        }

        protected override void Init() { }
        
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