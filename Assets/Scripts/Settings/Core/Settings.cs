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
        [Required, SerializeField] private TutorialSettings tutorialSettings = null;
        //DEVELOPMENT SETTINGS
        [Required, SerializeField, PropertyOrder(int.MaxValue)] private DevSettings devSettings = null;
        public static PlayerSettings Player => Instance.playerSettings;
        public static Audio.AudioSettings Audio => Instance.audioSettings;
        public static LevelSettings Level => Instance.levelSettings;
        public static DevSettings Dev => Instance.devSettings;
        public static TutorialSettings Tutorial => Instance.tutorialSettings;

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