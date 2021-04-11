using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    [HideMonoScript]
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance =>
            _instance ? _instance : throw new UnityException("No AudioController instance.");

        public AudioEvent Events => AudioEvent.Instance;
        private static AudioController _instance;
        private MasterAudio _masterAudio;
        
        public static bool Initialized
        {
            get
            {
                var initialized = _instance != null;
                if (!initialized)
                    Debug.LogWarning("No audio controller initialized.");
                    
                return initialized;
            }
        }

        public void Play(string soundGroupAttribute, float volume, float fadeTime)
        {
            if (fadeTime <= 0)
                MasterAudio.PlaySound(soundGroupAttribute, volume);
            else
            {
                MasterAudio.PlaySound(soundGroupAttribute, volume);
                MasterAudio.FadeSoundGroupToVolume(soundGroupAttribute, volume, fadeTime);
            }
        }
        
        public void Stop(string soundGroupAttribute, float fadeTime)
        {
            if (fadeTime <= 0)
                MasterAudio.StopAllOfSound(soundGroupAttribute);
            else
                MasterAudio.FadeSoundGroupToVolume(soundGroupAttribute, 0, fadeTime, null, true, true);
        }
        
        public void SetVolume(string soundGroupAttribute, float volume)
        {
            MasterAudio.SetGroupVolume(soundGroupAttribute, volume);
        }
        
        public void ChangePitch(string soundGroupAttribute, float pitch)
        {
            MasterAudio.ChangeVariationPitch(soundGroupAttribute, true, null, pitch);
        }
        
        public void MuteSFX(bool active)
        {
            if (active)
                MasterAudio.MuteBus("SFX");
            else
                MasterAudio.UnmuteBus("SFX");
        }
        
        public void MutePlaylist(bool active)
        {
            if (active)
                MasterAudio.MutePlaylist();
            else
                MasterAudio.UnmutePlaylist();
        }

        private void Awake()
        {
            if (_instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;
            _masterAudio = MasterAudio.Instance;
        }
    }

    public static class Sounds
    {
        private static bool Initialized => Check();
        private static AudioController _audioController;

        private static bool Check()
        {
            if (!AudioController.Initialized)
            {
                Debug.LogWarning("No audio controller found.");
                return false;
            }

            if (_audioController == null)
                _audioController = AudioController.Instance;

            return _audioController != null;
        }

        public static void MuteSFX(bool active)
        {
            if (Initialized)
                _audioController.MuteSFX(active);
        }
        
        public static void MutePlaylist(bool active)
        {
            if (Initialized)
                _audioController.MutePlaylist(active);
        }

        public static AudioEvent Event
        {
            get
            {
                if (Initialized)
                    return _audioController.Events;
                
                if (!_audioController)
                    throw new UnityException("No audio event controller found!");

                return _audioController.Events;
            }
        }

        public static void Play(string soundGroupAttribute, float volume = 1f, float fadeTime = 0)
        {
            if (Initialized)
                _audioController.Play(soundGroupAttribute, volume, fadeTime);
        }
        
        public static void Play(AudioSettings.SoundClip clip, float fadeTime = 0)
        {
            if (Initialized)
                _audioController.Play(clip.sound, clip.volume, fadeTime);
        }
        
        public static void SetVolume(string soundGroupAttribute, float volume)
        {
            if (Initialized)
                _audioController.SetVolume(soundGroupAttribute, Mathf.Clamp01(volume));
        }
        
        public static void ChangePitch(string soundGroupAttribute, float pitch)
        {
            if (Initialized)
                _audioController.ChangePitch(soundGroupAttribute, pitch);
        }
        
        public static void Stop(string soundGroupAttribute, float fadeTime = 0f)
        {
            if (Initialized)
                _audioController.Stop(soundGroupAttribute, fadeTime);
        }
    }
}