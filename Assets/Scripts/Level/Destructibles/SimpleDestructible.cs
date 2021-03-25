using System;
using Audio;
using DG.Tweening;
using UnityEngine;

namespace Level.Destructibles
{
    public class SimpleDestructible : MonoBehaviour
    {
        [SerializeField]
        private SimpleFX destroyFX = null;
        [SerializeField]
        private SpriteRenderer appearance = null;

        private bool _dying;

        private void Awake()
        {
            if (destroyFX)
            {
                destroyFX.gameObject.SetActive(false);
                destroyFX.OnEnd += OnEndFX;
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
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
    }
}
