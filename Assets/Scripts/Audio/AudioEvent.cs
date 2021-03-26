using UnityEngine.Events;

namespace Audio
{
    public sealed class AudioEvent
    {
        public static AudioEvent Instance => instance;
        private static readonly AudioEvent instance = new AudioEvent();
        private AudioEvent() { }
        static AudioEvent() { }

        public UnityEvent OnFootStep { get; } = new UnityEvent();
        public UnityEvent OnPlayerLand { get; } = new UnityEvent();
        public UnityEvent OnPlayerJump { get; } = new UnityEvent();
        public UnityEvent OnSwordSwoosh { get; } = new UnityEvent();
        public UnityEvent OnShieldHit { get; } = new UnityEvent();
        public UnityEvent OnDestructibleDestroy { get; } = new UnityEvent();


        public class AudioEventBool : UnityEvent<bool> {}
    }
}