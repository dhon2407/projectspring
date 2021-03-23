using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings.Core
{
    [CreateAssetMenu(fileName = "SettingsController", menuName = "Settings/Controller", order = 0)]
    public class Settings : ScriptableObject
    {
        public static bool Ready => _instance != null;
        
        [Required, SerializeField] private PlayerSettings playerSettings = null;
        [Required, SerializeField] private Audio.AudioSettings audioSettings = null;
        [Required, SerializeField] private LevelSettings levelSettings = null;
        // [Required, SerializeField] private UISettings uiSettings = null;
        // [Required, SerializeField] private CameraSettings cameraSettings = null;
        [Required, SerializeField, PropertyOrder(int.MaxValue)] private DevSettings devSettings = null;
        // [Required, SerializeField] private AudioSettings audioSettings = null;
        public static PlayerSettings Player => Instance.playerSettings;
        public static Audio.AudioSettings Audio => Instance.audioSettings;
        public static LevelSettings Level => Instance.levelSettings;
        // public static UISettings UI => Instance.uiSettings;
        // public static CameraSettings Camera => Instance.cameraSettings;
        public static DevSettings Dev => Instance.devSettings;
        // public static AudioSettings Audio => Instance.audioSettings;

        #region INSTANCE SETUP

        private static Settings _instance;
        private static Settings Instance => _instance ? _instance : Initialize();

        private static Settings Initialize()
        {
            _instance = Resources.Load<Settings>("SettingsController");
            return _instance;
        }

        #endregion
    }
}