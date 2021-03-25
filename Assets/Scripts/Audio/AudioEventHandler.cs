using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    [HideMonoScript]
    public class AudioEventHandler : MonoBehaviour
    {
        private AudioEvent _audioEvents;

        private AudioSettings SAudio => Settings.Core.Settings.Audio;

        private void Start()
        {
            _audioEvents = AudioEvent.Instance;
            SetupEvents();
        }

        private void SetupEvents()
        {
            _audioEvents.OnFootStep.AddListener(OnFootStep);
            _audioEvents.OnPlayerLand.AddListener(OnPlayerLand);
            _audioEvents.OnPlayerJump.AddListener(OnPlayerJump);
            _audioEvents.OnSwordSwoosh.AddListener(OnSwordSwoosh);
            _audioEvents.OnDestructibleDestroy.AddListener(OnDestructibleDestroy);
        }

        private void OnFootStep() => Sounds.Play(SAudio.footStep);
        private void OnPlayerLand() => Sounds.Play(SAudio.playerLand);
        private void OnPlayerJump() => Sounds.Play(SAudio.playerJump);
        private void OnSwordSwoosh() => Sounds.Play(SAudio.swordSwoosh);
        private void OnDestructibleDestroy() => Sounds.Play(SAudio.destructibleDestroyed);
    }
}