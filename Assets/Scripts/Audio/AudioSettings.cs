using System;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Audio", order = 0)]
    public class AudioSettings : ScriptableObject
    {
        [FoldoutGroup("Player")] [Title("Common")]
        [FoldoutGroup("Player")] public SoundClip footStep;
        [FoldoutGroup("Player")] public SoundClip playerJump;
        [FoldoutGroup("Player")] public SoundClip playerLand;
        [FoldoutGroup("Player")] public SoundClip swordSwoosh;
        [FoldoutGroup("Destructibles")] public SoundClip destructibleDestroyed;

        [Serializable]
        public struct SoundClip
        {
            [SoundGroup, HideLabel]
            public string sound;
            [Range(0f, 1f)]
            public float volume;
        }
        
#if UNITY_EDITOR
        [OnInspectorGUI, ShowIf("@NoMasterAudio"), PropertyOrder(-1)]
        private void Test()
        {
            UnityEditor.EditorGUILayout.HelpBox(
                "No Master Audio prefab on current scene. To easily configure audio settings, add a MasterAudio prefab from /Prefabs/Sounds folder.",
                UnityEditor.MessageType.Warning);
        }

        private bool NoMasterAudio => CheckMasterAudioComponent();

        private bool CheckMasterAudioComponent()
        {
            return FindObjectOfType<AudioController>() == null;
        }
#endif
    }
}