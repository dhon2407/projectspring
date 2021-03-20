using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Animation
{
    [AddComponentMenu("DMG/Animator/Landing Dust")]
    [RequireComponent(typeof(Animator))]
    [HideMonoScript]
    public class LandingDust : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Activate = Animator.StringToHash("activate");
        private PlayerController _player;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SetupEvents();
        }

        private void SetupEvents()
        {
            _player = GetComponentInParent<PlayerController>();
            if (_player)
                _player.OnGroundLand += Show;
        }

        private void OnDestroy()
        {
            if (_player)
                _player.OnGroundLand -= Show;
        }

        private void Show()
        {
            _animator.SetTrigger(Activate);
        }
    }
}