using Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class Options : MonoBehaviour
    {
        public static bool IsOpen { get; private set; }

        #region EVENTS

        public delegate void ChangeState(bool isOpen);
        public static event ChangeState OnOpen;

        #endregion
        
        [SerializeField]
        private BaseUIToggler bmgToggler = null;
        [SerializeField]
        private BaseUIToggler sfxToggler = null;

        [SerializeField]
        private GameObject title;
        [SerializeField]
        private GameObject buttons;
        [SerializeField]
        private GameObject credits;


        private CanvasGroup _canvasGroup;
        private Transform _transform;
        private bool _bgmMuted;
        private bool _sfxMuted;
        private bool _animating;
        private bool _shown;

        [SerializeField, ReadOnly]
        private string ppURL =
            "https://docs.google.com/document/d/1cOu4Z9VVM4ENfNpmJGliOhYyYuMvBVEtUcqNtLWth9U/edit?usp=sharing";
        
        public void ToggleShow()
        {
            if (_animating)
                return;
            
            if (_shown)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            _animating = true;
            _shown = true;
            OnOpen?.Invoke(_shown);
            
            Time.timeScale = 0;
            _transform.DOScale(Vector3.one, 0.5f).SetUpdate(true);
            _canvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                SetCanvasActive(true);
                _animating = false;
            }).SetUpdate(true);
        }

        public void Hide()
        {
            _animating = true;
            _shown = false;
            OnOpen?.Invoke(_shown);
            
            Time.timeScale = 1;
            _canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                SetCanvasActive(false);
                _animating = false;
                OnHideCredits();
            }).SetUpdate(true);
        }

        public void OnToggleBMG()
        {
            _bgmMuted = bmgToggler.Toggle();
            UpdateSoundSettings();
        }

        public void OnToggleSFX()
        {
            _sfxMuted = sfxToggler.Toggle();
            UpdateSoundSettings();
        }

        public void OnShowPrivacyPolicy()
        {
            Application.OpenURL(ppURL);
        }

        public void OnShowCredits()
        {
            credits.SetActive(true);
            title.SetActive(false);
            buttons.SetActive(false);
        }

        public void OnHideCredits()
        {
            credits.SetActive(false);
            title.SetActive(true);
            buttons.SetActive(true);
        }
        
        private void Awake()
        {
            _transform = transform;
            _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        private void Start()
        {
            LoadOptionValues();
        }

        private void LoadOptionValues()
        {
            _bgmMuted = 1 == PlayerPrefs.GetInt("BGMMuted", 0);
            _sfxMuted = 1 == PlayerPrefs.GetInt("SFXMuted", 0);

            bmgToggler.SetTo(_bgmMuted);
            sfxToggler.SetTo(_sfxMuted);

            UpdateSoundSettings();
        }

        private void UpdateSoundSettings()
        {
            PlayerPrefs.SetInt("BGMMuted", _bgmMuted ? 1 : 0);
            PlayerPrefs.SetInt("SFXMuted", _sfxMuted ? 1 : 0);
            
            Sounds.MuteSFX(_sfxMuted);
            Sounds.MutePlaylist(_bgmMuted);
        }

        private void SetCanvasActive(bool active)
        {
            _canvasGroup.interactable = active;
            _canvasGroup.blocksRaycasts = active;
            IsOpen = active;
        }
    }
}
