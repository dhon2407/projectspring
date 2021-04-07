using System.Collections.Generic;
using CustomHelper;
using Level;
using Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class LevelManager : SingletonManager<LevelManager>
    {
        private static int _levelSegmentGenerated;
        private readonly List<LevelSegmentHandler> _remainingSegments = new List<LevelSegmentHandler>();

        public static float MoveSpeedMultiplier => Instance._moveSpeedMultiplier;

        [ShowInInspector]
        private float _moveSpeedMultiplier;
        
        public static void RegisterSegment(LevelSegmentHandler levelSegmentHandler)
        {
            levelSegmentHandler.OnLevelSegmentEnding += SegmentEnding;
        }

        private static void SegmentEnding(LevelSegmentHandler segmentOwner, Transform endPoint)
        {
            _levelSegmentGenerated++;

            var nextSegmentRef = Instance.GetNextSegment();
            segmentOwner.OnLevelSegmentEnding -= SegmentEnding;
            
            var nextSegment = Instantiate(nextSegmentRef, endPoint.position,
                Quaternion.identity);
            nextSegment.name = $"{nextSegmentRef.name} - {_levelSegmentGenerated}";
            nextSegment.OnLevelSegmentStarting += owner =>
            {
                Instance.IncreaseMoveSpeed();
                if (segmentOwner != null)
                    segmentOwner.DestroySelf();
            };
        }

        private void IncreaseMoveSpeed()
        {
            _moveSpeedMultiplier =
                Mathf.Clamp(_moveSpeedMultiplier + Settings.Core.Settings.Level.speedRateIncreasePerSegment, 1,
                    Settings.Core.Settings.Level.maxPlayerSpeed);
        }

        protected override void Init()
        {
            _moveSpeedMultiplier = 1;
            _remainingSegments.AddRange(Settings.Core.Settings.Level.segmentList);

            GameManager.OnPlayerDie += () => _moveSpeedMultiplier = 1;
        }

        private LevelSegmentHandler GetNextSegment()
        {
            var nextSegment = _remainingSegments.GetRandom();
            _remainingSegments.Remove(nextSegment);
            
            if (_remainingSegments.Count == 0)
                _remainingSegments.AddRange(Settings.Core.Settings.Level.segmentList);

            return nextSegment;
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