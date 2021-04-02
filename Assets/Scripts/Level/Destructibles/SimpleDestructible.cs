using Audio;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Level.Destructibles
{
    public class SimpleDestructible : MonoBehaviour, IEntity
    {
        [SerializeField]
        private SimpleFX destroyFX = null;
        [SerializeField]
        private SpriteRenderer appearance = null;

        private bool _dying;
        private HurtBox _hurtBox;

        private void Awake()
        {
            _hurtBox = GetComponentInChildren<HurtBox>();
            _hurtBox.OnHit += Die;
            
            if (destroyFX)
            {
                destroyFX.gameObject.SetActive(false);
                destroyFX.OnEnd += OnEndFX;
            }
        }

        private void Die(HitBox hitBox)
        {
            if (_dying)
                return;
            
            _dying = true;
            Sounds.Event.OnDestructibleDestroy.Invoke();
            appearance.DOFade(0, 0.2f);
            if (destroyFX)
                destroyFX.gameObject.SetActive(true);
        }

        private void OnEndFX()
        {
            Destroy(gameObject);
        }

        public GameObject Owner => gameObject;
    }
}
