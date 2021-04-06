using UnityEngine;

namespace Player.Sensors
{
    public class PlayerSensor : BaseProximitySensor
    {
        public delegate void OnPlayerDetectEvent(PlayerController player);

        public event OnPlayerDetectEvent OnPlayerDetect;
        public event OnPlayerDetectEvent OnPlayerLeave;
            
        private void Awake()
        {
            SetTypeToDetect(new[] {typeof(PlayerController)});
            OnDetect += OnDetectPlayer;
            OnLeave += OnLeavePlayer;
        }

        private void OnDestroy()
        {
            OnDetect -= OnDetectPlayer;
            OnLeave -= OnLeavePlayer;
        }

        private void OnDetectPlayer(Component component)
        {
            if (component is PlayerController playerController)
                OnPlayerDetect?.Invoke(playerController);
        }
        
        private void OnLeavePlayer(Component component)
        {
            if (component is PlayerController playerController)
                OnPlayerLeave?.Invoke(playerController);
        }
    }
}